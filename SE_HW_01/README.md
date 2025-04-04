# КПО, Мини ДЗ №01

## Пояснения применения принципов SOLID.

## S – Single Responsibility

Каждый класс отвечает только за одну операцию:
1. Отдельные фабрики для создания конкретного животного или предмета.

2. Ветеринарная клиника работает автономно, т.е., например, не зависит от зоопарка.

3. Хранилище объектов отвечает за корректное хранение объектов,
т.е. контролирует корректность номера объекта.

4. Класс Zoo : IZoo выполняет управленческую роль: он ничего
не сохраняет или не проверяет здоровье животных. Вместо этого
он делегирует эти задачи специальным объектам.


## O — Open-Closed

Классы открыты для расширения, но закрыты для модификации:
1. Для добавления нового вида животного достаточно создать фабрику для данного животного.

2. Создана небольшая система исключений для зоопарка ZooException. Например при
добавлении животного вместо возвращения статус-кода кидается исключение, в котором
описывается, что случилось. Если в будущем понадобиться расширить спектр результатов,
то будет достаточным создать новый тип исключения.


## L — Liskov Substitution

Если P является подтипом Т, то любые объекты типа Т, присутствующие в программе,
могут заменяться объектами типа P без негативных последствий для функциональности программы:

1. В методе Main при добавлении животных и вещей в методы
AddAnimal и AddThing передаётся подтип IZoo. 

2. В методе Main при добавлении животных и вещей, методы AddAnimal и
AddThing передаётся подтип фабрик IAnimalFactory и IInventoryFactory. 

3. Объект Zoo использует подтип IInventoryStorage и IVetClinic.

4. Для всех инвенратных объектов используется подтип IInventory,
для всех животных - ещё и подтип Animal.


## I — Interface Segregation

Не следует ставить клиент в зависимость от методов, которые он не использует:
1. Это хорошо видно в иэрархии классов инвенторя: <br>
   <Конкретное животное> -> <Тип животного> -> Animal -> IInventory,IAlive <br>
   <Конкретная вещь> -> Thing -> IInventory <br>
   Для классификации животных и предметов как инвентарь используется тип IInventory <br>
   Для получения номера объекта используется тип IInventory <br>
   Для классификации животных используется тип Animal <br>
   Для получения потребляемой еды используется тип IAlive <br>


## D — Dependency Inversion

Модули верхнего уровня не должны зависеть от модулей нижнего уровня. И те,
и другие должны зависеть от абстракций. Абстракции не должны зависеть от
деталей. Детали должны зависеть от абстракций:
1. Отношение зоопарка и ветеринароной клиники - ассоциация.


## Юнит Тесты

Были написаны тесты для всех функциональных классов (кроме интерфейсов Interfaces и
моделей Models, для них писать тесты нет смысла).

Итого получилось 25 тестов.
