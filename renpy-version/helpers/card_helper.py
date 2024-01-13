from random import randrange

from entities.card_type import CardType

def generateRandomCard():
    return CardType(randrange(1, 4))

def generateStartingHand():
    return [generateRandomCard() for i in range(3)]

def parse_card(card):
    match card.str.lower():
        case "rock": return CardType.Rock
        case "paper": return CardType.Paper,
        case "scissors": return CardType.Scissors,
        case _: return CardType.Unknown,