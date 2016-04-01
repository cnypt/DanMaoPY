class Bank(): # let's create a bank, building ATMs
    crisis = False
    def create_atm(self) :
       while not self.crisis :
           yield "$100"
hsbc = Bank() # when everything's ok the ATM gives you as much as you want
corner_street_atm = hsbc.create_atm()
print(corner_street_atm.next())