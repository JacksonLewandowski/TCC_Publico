#include "main.h"
#include "esp_now.h"
#include <WiFi.h>

#define RPM_CONSTANT 15000000 // 60 segundos * 1s em us * 1 rotação / 4 pulsos por volta

// ESP8266 Board MAC Address:  48:3F:DA:5A:06:4B
uint8_t partner_mac[6] = {0x48, 0x3F, 0xDA, 0x5A, 0x06, 0x4B};
esp_now_peer_info_t peer_info;
const uint8_t ma_period = 4;
uint32_t rpm = 0;
uint32_t v_rpm[4] = {0};
uint8_t i_rpm = 0;

void esp_now_write(unsigned char *data, unsigned short dimension);

void esp_now_recv_cb(const uint8_t *mac_addr, const uint8_t *data, int data_len)
{
    processMessage(data, data_len);
}

void setup()
{
    WiFi.mode(WIFI_STA);
    Serial.begin(115200);

    if (esp_now_init() != 0)
    {
        Serial.println("Error initializing ESP-NOW");
        return;
    }

    peer_info.channel = 0;
    peer_info.encrypt = false;
    memcpy(peer_info.peer_addr, partner_mac, 6);

    esp_now_add_peer(&peer_info);

    esp_now_register_recv_cb(esp_now_recv_cb);

    pwmArmature.reset();
    pwmField.reset();
    EEPROM.begin(eepromSize);
    // dacWrite(pinRefArmatureCurrent,50);
    //  initialize_uart();
    initFunctions(esp_now_write);
    initRegisters();
    initTimers();
    armaturePID = new PID(VetRegisters[HoldingRegOffset + RegAddressKp]->read(),
                          VetRegisters[HoldingRegOffset + RegAddressKi]->read(),
                          VetRegisters[HoldingRegOffset + RegAddressKd]->read());
    //armaturePID = new PID(200, 2000, 10);
    delay(1000);

    ZeroCurrent = ArmatureCurrent.read(); // faz a leitura da corrente com tudo desligado
}
void loop()
{
    unsigned short state = VetRegisters[HoldingRegOffset + RegAddressState]->read();
    switch (state)
    {
    case StateIdle:
        stateIdle();
        break;
    case StateRunning:
        stateRunning();
        break;
    case StateFailure:
        stateFailure();
        break;
    }
}
void stateIdle()
{
    VetRegisters[HoldingRegOffset + RegAddressPWMarmature]->write(0);
    rpm = 0;
    pwmArmature.reset();
    pwmField.reset();
    RPMcounter = 0;
    dacWrite(pinRefArmatureCurrent, 0);
    previousState = StateIdle;
    
    armaturePID->restart();
}

uint8_t last_state_encoder = 0;
unsigned long last_time = 0;
unsigned long last_time_high = 0;


void stateRunning()
{
    uint8_t state_encoder = digitalRead(pinReadRPM);
    unsigned long time = micros();

    if (state_encoder != last_state_encoder)
    {
        if (!state_encoder && (time - last_time_high) > MinumumHighStateTime)
        {
            unsigned long period = time - last_time;
            // rpm = (rpm * 3 + (RPM_CONSTANT / period)) / 4;
            v_rpm[i_rpm] = (RPM_CONSTANT / period);
            i_rpm = (i_rpm+1)%ma_period;

            for(int i = 0; i < ma_period; i++)
                rpm += v_rpm[i];

            rpm = rpm / (ma_period*1.26);
            last_time = time;

            VetRegisters[HoldingInputOffset + RegAddressRPMread]->write(rpm);            
        }
        if (state_encoder) // se está em nível alto
        {
            last_time_high = time;
        }
        last_state_encoder = state_encoder;
    }

    previousState = StateRunning;

    if (VetRegisters[HoldingRegOffset + RegAddressRPM]->read())
        Automatic();
    else
    {
        Manual();
        armaturePID->restart();
    }
        
}
void stateFailure()
{
    /*failure*/
    pwmArmature.reset();
    pwmField.reset();
    dacWrite(pinRefArmatureCurrent, 0);
    previousState = StateFailure;
}
void Manual()
{
    unsigned short valuePWMarmature = VetRegisters[HoldingRegOffset + RegAddressPWMarmature]->read();
    if (valuePWMarmature > PWMout::PWM_MAX)
    {
        valuePWMarmature = PWMout::PWM_MAX;
        VetRegisters[HoldingRegOffset + RegAddressPWMarmature]->write(valuePWMarmature);
    }
    pwmArmature.write(valuePWMarmature);
    previousState = StateRunning;
}
void Automatic()
{
    pid();
}
unsigned long last_time_pid = 0;
unsigned int count = 0;
void pid()
{
    unsigned long time = micros();
    // // if (ISR_PIDFlag == 0)
    // //     return;
    // // ISR_PIDFlag = 0;
    if (time - last_time_pid < DELTA_T_MS * Timer_1ms)
        return;

    
    count++;
    if(count > 150)
    {
        armaturePID->restart();
        count = 0;
    }

    last_time_pid = time;

    armaturePID->setPID(VetRegisters[HoldingRegOffset + RegAddressKp]->read(),
                        VetRegisters[HoldingRegOffset + RegAddressKi]->read(),
                        VetRegisters[HoldingRegOffset + RegAddressKd]->read());

    uint16_t valuePWM = VetRegisters[HoldingRegOffset + RegAddressPWMarmature]->read();
    if(count > 100) 
    {
        valuePWM = 0;
        rpm = 0;
        for(int i = 0; i < ma_period; i++)
            v_rpm[i] = 0;
    }
    else
        valuePWM = armaturePID->calculatePID(VetRegisters[HoldingRegOffset + RegAddressRPM]->read(),
                                          rpm);
    if (valuePWM > PWMout::PWM_MAX) valuePWM = PWMout::PWM_MAX;

    if (valuePWM < 0) valuePWM = 0;

    VetRegisters[HoldingRegOffset + RegAddressPWMarmature]->write(valuePWM);
    pwmArmature.write(valuePWM);
    
}
// essa função impede que a interrupção de proteção de sobrecorrente aconteça na partida.
void changeState()
{
    if (previousState == 1)
    {
        dacWrite(pinRefArmatureCurrent, DefaultStartCurrent);
        delay(10);
        attachInterrupt(pinArmatureOvercurrent, &ISR_Overcurrent, RISING);
        timeStartRunning = millis();
    }
    if ((millis() - timeStartRunning) > 5)
        dacWrite(pinRefArmatureCurrent, DefaultIdleCurrent);
}

void esp_now_write(unsigned char *data, unsigned short dimension)
{
    unsigned char reply[dimension + 2];
    memcpy(reply, data, dimension);
    unsigned short _crc = returnCRC16(data, dimension);
    LittleEndianConvert(&reply[dimension], _crc);
    esp_now_send(peer_info.peer_addr, reply, dimension + sizeof(_crc));
}
