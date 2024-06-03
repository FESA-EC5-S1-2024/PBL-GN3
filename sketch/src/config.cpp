#include "../include/config.hpp"

void reconnectMQTT();

WiFiClient espClient;
PubSubClient MQTT(espClient);

void initSerial() { Serial.begin(115200); }

void VerificaConexoesWiFIEMQTT() {
  if (!MQTT.connected()) {
    reconnectMQTT();
  }
}

void InitOutput() {
  pinMode(D4, OUTPUT);
  digitalWrite(D4, HIGH);
  boolean toggle = false;

  for (int i = 0; i <= 10; i++) {
    toggle = !toggle;
    digitalWrite(D4, toggle);
    delay(200);
  }
}
