#include "../include/WiFi.hpp"

#include <WiFiManager.h>

void initWiFi() {
  WiFiManager wm;

  bool res;

  res = wm.autoConnect("PBL-GN3", "password");

  if (!res) {
    Serial.println("Failed to connect");
    ESP.restart();
  } else {
    Serial.println("connected...yeey :)");
  }
}
