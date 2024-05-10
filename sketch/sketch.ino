#include <PubSubClient.h>
#include <WiFi.h>

// Configurações - variáveis editáveis
const char *default_SSID = "nome"; // Nome da rede Wi-Fi
const char *default_PASSWORD = "senha";  // Senha da rede Wi-Fi
const char *default_BROKER_MQTT = "servidor"; // IP do Broker MQTT
const int default_BROKER_PORT = 1883;            // Porta do Broker MQTT
const char *default_TOPICO_SUBSCRIBE =
    "/TEF/temp003/cmd"; // Tópico MQTT de escuta
const char *default_TOPICO_PUBLISH_1 =
    "/TEF/temp003/attrs"; // Tópico MQTT de envio de informações para Broker
const char *default_TOPICO_PUBLISH_2 =
    "/TEF/temp003/attrs/t"; // Tópico MQTT de envio de informações para
                                // Broker
const char *default_ID_MQTT = "fiware_002"; // ID MQTT
const int default_D4 = 2;                   // Pino do LED onboard
// Declaração da variável para o prefixo do tópico
const char *topicPrefix = "temp003";

// Variáveis para configurações editáveis
char *SSID = const_cast<char *>(default_SSID);
char *PASSWORD = const_cast<char *>(default_PASSWORD);
char *BROKER_MQTT = const_cast<char *>(default_BROKER_MQTT);
int BROKER_PORT = default_BROKER_PORT;
char *TOPICO_SUBSCRIBE = const_cast<char *>(default_TOPICO_SUBSCRIBE);
char *TOPICO_PUBLISH_1 = const_cast<char *>(default_TOPICO_PUBLISH_1);
char *TOPICO_PUBLISH_2 = const_cast<char *>(default_TOPICO_PUBLISH_2);
char *ID_MQTT = const_cast<char *>(default_ID_MQTT);
int D4 = default_D4;

WiFiClient espClient;
PubSubClient MQTT(espClient);
char EstadoSaida = '0';

void initSerial() { Serial.begin(115200); }

void initWiFi() {
  delay(10);
  Serial.println("------Conexao WI-FI------");
  Serial.print("Conectando-se na rede: ");
  Serial.println(SSID);
  Serial.println("Aguarde");
  reconectWiFi();
}

void initMQTT() {
  MQTT.setServer(BROKER_MQTT, BROKER_PORT);
  MQTT.setCallback(mqtt_callback);
}

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
  EnviaEstadoOutputMQTT();
  handleTemperature();
  MQTT.loop();
  delay(1000);
}

void reconectWiFi() {
  if (WiFi.status() == WL_CONNECTED)
    return;
  WiFi.begin(SSID, PASSWORD);
  while (WiFi.status() != WL_CONNECTED) {
    delay(100);
    Serial.print(".");
  }
  Serial.println();
  Serial.println("Conectado com sucesso na rede ");
  Serial.print(SSID);
  Serial.println("IP obtido: ");
  Serial.println(WiFi.localIP());

  // Garantir que o LED inicie desligado
  digitalWrite(D4, LOW);
}

void mqtt_callback(char *topic, byte *payload, unsigned int length) {
  String msg;
  for (int i = 0; i < length; i++) {
    char c = (char)payload[i];
    msg += c;
  }
  Serial.print("- Mensagem recebida: ");
  Serial.println(msg);

  // Forma o padrão de tópico para comparação
  String onTopic = String(topicPrefix) + "@on|";
  String offTopic = String(topicPrefix) + "@off|";

  // Compara com o tópico recebido
  if (msg.equals(onTopic)) {
    digitalWrite(D4, HIGH);
    EstadoSaida = '1';
  }

  if (msg.equals(offTopic)) {
    digitalWrite(D4, LOW);
    EstadoSaida = '0';
  }
}

void VerificaConexoesWiFIEMQTT() {
  if (!MQTT.connected())
    reconnectMQTT();
  reconectWiFi();
}

void EnviaEstadoOutputMQTT() {
  if (EstadoSaida == '1') {
    MQTT.publish(TOPICO_PUBLISH_1, "s|on");
    Serial.println("- Led Ligado");
  }

  if (EstadoSaida == '0') {
    MQTT.publish(TOPICO_PUBLISH_1, "s|off");
    Serial.println("- Led Desligado");
  }
  Serial.println("- Estado do LED onboard enviado ao broker!");
  delay(1000);
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

void reconnectMQTT() {
  while (!MQTT.connected()) {
    Serial.print("* Tentando se conectar ao Broker MQTT: ");
    Serial.println(BROKER_MQTT);
    if (MQTT.connect(ID_MQTT)) {
      Serial.println("Conectado com sucesso ao broker MQTT!");
      MQTT.subscribe(TOPICO_SUBSCRIBE);
    } else {
      Serial.println("Falha ao reconectar no broker.");
      Serial.println("Haverá nova tentativa de conexão em 2s");
      delay(2000);
    }
  }
}

void handleTemperature() {
  const int analogPin = 34; // Pino analógico utilizado para a leitura da tensão
  int sensorValue = analogRead(analogPin); // Lê o valor analógico do sensor
  float temperature = (sensorValue * (100.0 / 4095.0)) +
                      9.3; // Converte o valor para tensão (0-3.3V)

  // Envia a temperatura para o tópico MQTT correspondente
  char message[10];
  snprintf(message, 10, "%.2f",
           temperature); // Formata a temperatura com 2 casas decimais
  MQTT.publish(TOPICO_PUBLISH_2, message);

  // Exibe a temperatura no monitor serial
  Serial.print("Temperatura: ");
  Serial.print(temperature);

  Serial.println(" graus Celsius");
  Serial.print(sensorValue);
}
