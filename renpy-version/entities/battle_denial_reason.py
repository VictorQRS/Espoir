from enum import Enum

class BattleDenialReason(Enum):
    Player1LackStars = 0,
    Player2LackStars = 1,
    PlayersCanNotFightThemselves = 2,