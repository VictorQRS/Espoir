from collections import Counter
from entities.non_playable_player import NonPlayablePlayer
from entities.player import Player

MAX_PLAYERS = 100

class EspoirContext:
    def __init__(self):
        self.MainCharacter = Player()
        self.Players = [self.MainCharacter]
        for i in range(1, MAX_PLAYERS):
            self.Players.append(NonPlayablePlayer(i))
        
        # TODO: 10-min task

    def startGame():
        pass

    def finishGame():
        pass

    def getCardFrequency(self):
        return Counter(self.Cards)