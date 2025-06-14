# ВШЭ, 2-й курс, Конструирование программного обеспечения

<img alt="KPO" src="./docs/kpo.jpg" style="width: 400px" />

---

## Мини-ДЗ:
1. [Мини-ДЗ 01](https://github.com/Mi-Kram/HSE-Software-Engineering/tree/main/SE_HW_01)
: Московский зоопарк (SOLID)
2. [Мини-ДЗ 02](https://github.com/Mi-Kram/HSE-Software-Engineering/tree/main/SE_HW_02)
: Московский зоопарк (Clean Architecture)

## Контрольные Работы:
1. [КР 01](https://github.com/Mi-Kram/HSE-Software-Engineering/tree/main/SE_CW_01)
: Hse Bank (Паттерны)
2. [КР 02](https://github.com/Mi-Kram/HSE-Software-Engineering/tree/main/SE_CW_02)
: Анализ текстовых файлов (Микросервисная архитектура)
3. [КР 03](https://github.com/Mi-Kram/HSE-Software-Engineering/tree/main/SE_CW_03)
: Оплата заказов (Взаимодействие сервисов через Kafka)

---

# Визуалицая покрытия кода Unit тестами

1. Скачать необходимые пакеты:
   ```
   dotnet add package coverlet.collector
   ```
   ```
   dotnet tool install -g dotnet-reportgenerator-globaltool
   ```

2. Открыть терминал в корне проекта и выполнить команду:
   
   2.1. Запустить тесты всех тестовых проектов:
      ```
      dotnet test --collect "XPlat Code Coverage"
      ```
   
   2.2. Запустить тесты одного тестового проекта:
      ```
      dotnet test ./<project>.Tests/<project>.Tests.csproj --collect "XPlat Code Coverage"
      ```

3. Открыть терминал в каталоге `<solution>/<project>.Tests/TestReults/<hash>` и выполнить команду:
   ```
   reportgenerator -reports:coverage.cobertura.xml -reportTypes:Html -targetDir:./html/
   ```

4. Для отображения результата запустить файл `<solution>/<project>.Tests/TestReults/<hash>/html/index.html`


