#include "../include/WiFi.hpp" // Inclui o arquivo de cabeçalho WiFi.hpp

#include <WiFiManager.h> // Inclui a biblioteca WiFiManager

// Inicializa a conexão WiFi
void initWiFi() {
  WiFiManager wm; // Cria uma instância de WiFiManager

  bool res; // Variável para armazenar o resultado da conexão

  // Tenta conectar à rede WiFi usando o SSID e a senha especificados.
  // Se não conseguir se conectar, reinicia o ESP32.
  res = wm.autoConnect("PBL-GN3", "password");

  if (!res) {
    Serial.println("Failed to connect"); // Exibe mensagem de falha na conexão
    ESP.restart();                       // Reinicia o ESP32
  } else {
    Serial.println(
        "connected...yeey :)"); // Exibe mensagem de sucesso na conexão
  }
}
