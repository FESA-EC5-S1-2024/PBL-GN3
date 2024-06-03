#include "../include/MQTT.hpp"   // Inclui o arquivo de cabeçalho MQTT.hpp
#include "../include/config.hpp" // Inclui o arquivo de configuração

// Inicializa a conexão com o servidor MQTT
void initMQTT() {
  MQTT.setServer(BROKER_MQTT,
                 BROKER_PORT); // Configura o servidor MQTT e a porta
}

// Reconecta ao servidor MQTT
void reconnectMQTT() {
  // Enquanto não estiver conectado ao servidor MQTT
  while (!MQTT.connected()) {
    Serial.print("* Tentando se conectar ao Broker MQTT: ");
    Serial.println(BROKER_MQTT);

    // Se conseguir conectar ao servidor MQTT
    if (MQTT.connect(ID_MQTT)) {
      Serial.println("Conectado com sucesso ao broker MQTT!");
      MQTT.subscribe(TOPICO_SUBSCRIBE); // Inscreve-se no tópico MQTT
    } else {
      Serial.println("Falha ao reconectar no broker.");
      Serial.println("Haverá nova tentativa de conexão em 2s");
      delay(2000); // Aguarda 2 segundos antes de tentar reconectar
    }
  }
}
