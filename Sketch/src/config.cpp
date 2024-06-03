#include "../include/config.hpp" // Inclui o arquivo de configuração

// Função para reconectar ao MQTT
void reconnectMQTT();

// Cliente WiFi e MQTT
WiFiClient espClient;
PubSubClient MQTT(espClient);

// Inicializa a comunicação serial
void initSerial() { Serial.begin(115200); }

// Verifica as conexões WiFi e MQTT e reconecta se necessário
void VerificaConexoesWiFIEMQTT() {
  if (!MQTT.connected()) {
    reconnectMQTT();
  }
}

// Inicializa as saídas do sistema
void InitOutput() {
  pinMode(D4, OUTPUT);    // Configura o pino D4 como saída
  digitalWrite(D4, HIGH); // Define o estado inicial do pino D4 como HIGH
  boolean toggle = false; // Variável para alternar o estado do pino D4

  // Pisca o LED onboard 10 vezes
  for (int i = 0; i <= 10; i++) {
    toggle = !toggle;         // Alterna o estado do pino D4
    digitalWrite(D4, toggle); // Define o estado do pino D4
    delay(200);               // Aguarda 200ms
  }
}
