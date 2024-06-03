#pragma once

#include <PubSubClient.h>
#include <WiFi.h>

// Configurações - variáveis editáveis
#define BROKER_MQTT ("servidor")              // IP do Broker MQTT
#define BROKER_PORT (1883)                    // Porta do Broker MQTT
#define TOPICO_SUBSCRIBE ("/TEF/temp003/cmd") // Tópico MQTT de escuta
#define TOPICO_PUBLISH                                                         \
  ("/TEF/temp003/attrs/t") // Tópico MQTT de envio de informações para o Broker
#define ID_MQTT ("fiware_002")
#define topicPrefix                                                            \
  ("temp003") // Declaração da variável para o prefixo do tópico

#define D4 2 // Pino do LED onboard
#define TEMPERATURE_PIN 34
#define SAMPLES 50 // Cada Sample é 50ms

extern WiFiClient espClient; // Cliente WiFi
extern PubSubClient MQTT;    // Cliente MQTT

// Inicializa a comunicação serial
void initSerial();

// Verifica as conexões WiFi e MQTT
void VerificaConexoesWiFIEMQTT();

// Inicializa as saídas do sistema
void InitOutput();
