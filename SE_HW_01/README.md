# ���, ���� �� �01

## ��������� ���������� ��������� SOLID.

## S � Single Responsibility

������ ����� �������� ������ �� ���� ��������:
1. ��������� ������� ��� �������� ����������� ��������� ��� ��������.

2. ������������ ������� �������� ���������, �.�., ��������, �� ������� �� ��������.

3. ��������� �������� �������� �� ���������� �������� ��������,
�.�. ������������ ������������ ������ �������.

4. ����� Zoo : IZoo ��������� �������������� ����: �� ������
�� ��������� ��� �� ��������� �������� ��������. ������ �����
�� ���������� ��� ������ ����������� ��������.


## O � Open-Closed

������ ������� ��� ����������, �� ������� ��� �����������:
1. ��� ���������� ������ ���� ��������� ���������� ������� ������� ��� ������� ���������.

2. ������� ��������� ������� ���������� ��� �������� ZooException. �������� ���
���������� ��������� ������ ����������� ������-���� �������� ����������, � �������
�����������, ��� ���������. ���� � ������� ������������ ��������� ������ �����������,
�� ����� ����������� ������� ����� ��� ����������.


## L � Liskov Substitution

���� P �������� �������� �, �� ����� ������� ���� �, �������������� � ���������,
����� ���������� ��������� ���� P ��� ���������� ����������� ��� ���������������� ���������:

1. � ������ Main ��� ���������� �������� � ����� � ������
AddAnimal � AddThing ��������� ������ IZoo. 

2. � ������ Main ��� ���������� �������� � �����, ������ AddAnimal �
AddThing ��������� ������ ������ IAnimalFactory � IInventoryFactory. 

3. ������ Zoo ���������� ������ IInventoryStorage � IVetClinic.

4. ��� ���� ����������� �������� ������������ ������ IInventory,
��� ���� �������� - ��� � ������ Animal.


## I � Interface Segregation

�� ������� ������� ������ � ����������� �� �������, ������� �� �� ����������:
1. ��� ������ ����� � �������� ������� ���������: <br>
   <���������� ��������> -> <��� ���������> -> Animal -> IInventory,IAlive <br>
   <���������� ����> -> Thing -> IInventory <br>
   ��� ������������� �������� � ��������� ��� ��������� ������������ ��� IInventory <br>
   ��� ��������� ������ ������� ������������ ��� IInventory <br>
   ��� ������������� �������� ������������ ��� Animal <br>
   ��� ��������� ������������ ��� ������������ ��� IAlive <br>


## D � Dependency Inversion

������ �������� ������ �� ������ �������� �� ������� ������� ������. � ��,
� ������ ������ �������� �� ����������. ���������� �� ������ �������� ��
�������. ������ ������ �������� �� ����������:
1. ��������� �������� � ������������� ������� - ����������.


## ���� �����

���� �������� ����� ��� ���� �������������� ������� (����� ����������� Interfaces �
������� Models, ��� ��� ������ ����� ��� ������).

����� ���������� 25 ������.
