#pragma once

#include <PubSubClient.h>
#include <WiFi.h>

// Configurações - variáveis editáveis
#define BROKER_MQTT ("servidor")              // IP do Broker MQTT
#define BROKER_PORT (1883)                    // Porta do Broker MQTT
#define TOPICO_SUBSCRIBE ("/TEF/temp003/cmd") // Tópico MQTT de escuta
#define TOPICO_PUBLISH                                                         \
  ("/TEF/temp003/attrs/t")     // Tópico MQTT de envio de informações para
                               // Broker
#define ID_MQTT ("fiware_002") // ID MQTT
#define topicPrefix                                                            \
  ("temp003") // Declaração da variável para o prefixo do tópico

#define D4 2 // Pino do LED onboard
#define TEMPERATURE_PIN 34
#define SAMPLES 50

extern WiFiClient espClient;
extern PubSubClient MQTT;

void initSerial();
void VerificaConexoesWiFIEMQTT();
void InitOutput();
