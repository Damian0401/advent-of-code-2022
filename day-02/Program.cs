
Console.WriteLine("It is a good day to be a human.");

// PartOne();
// PartTwo();

#pragma warning disable

void PartOne()
{
    var data = File.ReadAllLines("data.txt");

    var rounds = data.Select(x => 
        new Round(TranslateMove(x[0]), TranslateMove(x[2])));

    var result = rounds
        .Select(x => CalculateScore(x))
        .Sum();

    File.WriteAllText("result.txt", result.ToString());
}

void PartTwo()
{
    var data = File.ReadAllLines("data.txt");

    var games = data.Select(x =>
        new Game(TranslateMove(x[0]), TranslateResult(x[2])));

    var result = games
        .Select(x => CalculateScoreWithPrediction(x))
        .Sum();

    File.WriteAllText("result.txt", result.ToString());
}

int CalculateScore(Round round)
{
    int score = (int)round.YourMove;

    if (round.OpponentMove.Equals(round.YourMove))
        score += 3;

    if (round.YourMove.Equals(Move.Rock) && round.OpponentMove.Equals(Move.Scissors))
        score += 6;

    if (round.YourMove.Equals(Move.Paper) && round.OpponentMove.Equals(Move.Rock))
        score += 6;

    if (round.YourMove.Equals(Move.Scissors) && round.OpponentMove.Equals(Move.Paper))
        score += 6;

    return score;
}

int CalculateScoreWithPrediction(Game game)
{
    Move yourMove = PredictMove(game);

    Round round = new Round(game.OpponentMove, yourMove);
    int score = CalculateScore(new Round(game.OpponentMove, yourMove));

    return score;
}

Move PredictMove(Game game)
{
    if (game.Result.Equals(Result.Draw))
        return game.OpponentMove;

    if (game.Result.Equals(Result.Win))
    {
        if (game.OpponentMove.Equals(Move.Rock))
            return Move.Paper;

        if (game.OpponentMove.Equals(Move.Paper))
            return Move.Scissors;

        return Move.Rock;
    }

    if (game.OpponentMove.Equals(Move.Rock))
        return Move.Scissors;

    if (game.OpponentMove.Equals(Move.Paper))
        return Move.Rock;

    return Move.Paper;
}

Move TranslateMove(char move)
{
    return move switch
    {
        'X' or 'A' => Move.Rock,
        'Y' or 'B' => Move.Paper,
        'Z' or 'C' => Move.Scissors,
    };
}

Result TranslateResult(char result)
{
    return result switch
    {
        'X' => Result.Lose,
        'Y' => Result.Draw,
        'Z' => Result.Win,
    };
}

record Round(Move OpponentMove, Move YourMove);
record Game(Move OpponentMove, Result Result);

enum Move
{
    Rock = 1,
    Paper = 2,
    Scissors = 3
}

enum Result
{
    Win = 6,
    Draw = 3,
    Lose = 0
}
