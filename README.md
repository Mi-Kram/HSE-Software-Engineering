# ВШЭ, 2-й курс, Конструирование программного обеспечения

<br>

# Группа БПИ234-1, Крамаренко Михаил Константинович 

**ТГ**: [@Mikhail_0811](https://t.me/Mikhail_0811)

---

## Мини-ДЗ:
1. [Мини-ДЗ 01](https://github.com/Mi-Kram/HSE-Software-Engineering/tree/main/SE_HW_01)

## Контрольные Работы:
1. [КР 01](https://github.com/Mi-Kram/HSE-Software-Engineering/tree/main/SE_CW_01)

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
   ```
   dotnet test --collect "XPlat Code Coverage"
   ```

3. Открыть терминал в каталоге `<solution>/<project>.Tests/TestReults/<hash>` и выполнить команду:
   ```
   reportgenerator -reports:coverage.cobertura.xml -reportTypes:Html -targetDir:./html/
   ```

4. Для отображения результата запустить файл `<solution>/<project>.Tests/TestReults/<hash>/html/index.html`


