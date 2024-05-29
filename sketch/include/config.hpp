#pragma once

#include <PubSubClient.h>
#include <WiFi.h>

// Configurações - variáveis editáveis
#define SSID ("nome") // Nome da rede Wi-Fi
#define PASSWORD ("senha")  // Senha da rede Wi-Fi
#define BROKER_MQTT ("servidor") // IP do Broker MQTT
#define BROKER_PORT (1883)            // Porta do Broker MQTT
#define TOPICO_SUBSCRIBE ("/TEF/temp003/cmd") // Tópico MQTT de escuta
#define TOPICO_PUBLISH_1 ("/TEF/temp003/attrs") // Tópico MQTT de envio de informações para Broker
#define TOPICO_PUBLISH_2 ("/TEF/temp003/attrs/t") // Tópico MQTT de envio de informações para
                                // Broker
#define ID_MQTT ("fiware_002") // ID MQTT
                               //
#define D4 (2)                   // Pino do LED onboard

// Declaração da variável para o prefixo do tópico
#define topicPrefix ("temp003")

#define TEMPERATURE_PIN 34
#define SAMPLES 50
extern WiFiClient espClient;
extern PubSubClient MQTT;

void initSerial();
void VerificaConexoesWiFIEMQTT();
void InitOutput();
