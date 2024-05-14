#include "include/config.hpp"
#include "include/measurements.hpp"
#include "include/MQTT.hpp"
#include "include/WI-FI.hpp"

void setup() {
  InitOutput();
  initSerial();
  initWiFi();
  initMQTT();
  delay(5000);
  MQTT.publish(TOPICO_PUBLISH_1, "s|on");
}

void loop() {
  VerificaConexoesWiFIEMQTT();
  readTemperature();
  MQTT.loop();
  delay(1000);
}
