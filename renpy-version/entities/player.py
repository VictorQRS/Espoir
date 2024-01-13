from collections import Counter

class Player:
    def __init__(self):
        self.Name = ""
        self.WarFunds = 0
        self.Debt = 0
        self. Stars = 3
        self.Cards = []
    
    def lookAtCards(self):
        groupedCards = Counter(self.Cards)
        groupedCardsStr = ",".join(groupedCards.map(lambda e: f"{groupedCards[e]}x{e}"))
        return f'You have {groupedCardsStr}'
    
    def removeCard(self, card):
        self.Cards.remove(card)