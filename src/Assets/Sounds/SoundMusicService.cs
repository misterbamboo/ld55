using System.Collections.Generic;
using System.IO;
using System.Linq;
using Godot;

public partial class SoundMusicService : Node
{
    private const string ArcanicFocusSlide = "ArcanicFocusSlide";
    private const string CandleLit = "CandleLit";
    private const string CardMoved = "CardMoved";
    private const string CardPlaced = "CardPlaced";
    private const string CardSummoned = "CardSummoned";
    private const string CardWon = "CardWon";
    private const string Draw = "Draw";
    private const string GameOver = "GameOver";
    private const string Hit = "Hit";
    private const string ItemDropped = "ItemDropped";
    private const string ItemGrabbed = "ItemGrabbed";
    private const string Lose = "Lose";
    private const string StartGame = "StartGame";
    private const string WinCritical = "Win-Critical";
    private const string Win = "Win";

    [Export] private AudioStreamPlayer[] musicPlayers;
    [Export] private Godot.Collections.Array<AudioStreamWav> ResourceMusics;

    private Dictionary<string, AudioStreamWav> Musics { get; } = new Dictionary<string, AudioStreamWav>();
    private DeskManager DeskManager { get; set; }

    public override void _Ready()
    {
        foreach (var resourceMusics in ResourceMusics)
        {
            string key = ExtractKey(resourceMusics);
            Musics[key] = resourceMusics;
        }

        RegisterEvents();
    }

    public override void _Process(double delta)
    {
    }

    private static string ExtractKey(AudioStreamWav resourceMusics)
    {
        return Path.GetFileNameWithoutExtension(resourceMusics.ResourcePath);
    }

    private void RegisterEvents()
    {
        DeskManager = GetNode<DeskManager>(DeskManager.Path);
        DeskManager.OnGameStart += DeskManager_OnGameStart;
        DeskManager.OnArcaneFocusDragStart += DeskManager_OnArcaneFocusDragStart;
        DeskManager.OnCandleToggled += DeskManager_OnCandleToggled;
        DeskManager.OnMonsterSummoned += DeskManager_OnMonsterSummoned;
        DeskManager.OnFightStarting += DeskManager_OnFightStarting;
        DeskManager.OnItemGrabbed += DeskManager_OnItemGrabbed;
        DeskManager.OnItemDropped += DeskManager_OnItemDropped;
        DeskManager.OnFightCompleted += DeskManager_OnFightCompleted;
        DeskManager.OnCardMoving += DeskManager_OnCardMoving; ; 
    }

    private void DeskManager_OnGameStart()
    {
        TryPlaying(StartGame);
    }

    private void DeskManager_OnArcaneFocusDragStart()
    {
        TryPlaying(ArcanicFocusSlide);
    }

    private void DeskManager_OnCandleToggled(bool state)
    {
        if (state)
        {
            TryPlaying(CandleLit);
        }
    }

    private void DeskManager_OnMonsterSummoned(SummoningSpecs monster)
    {
        TryPlaying(CardSummoned);
    }

    private void DeskManager_OnFightStarting()
    {
        TryPlaying(CardPlaced);
    }

    private void DeskManager_OnItemGrabbed()
    {
        TryPlaying(ItemGrabbed);
    }

    private void DeskManager_OnItemDropped()
    {
        TryPlaying(ItemDropped);
    }

    private void DeskManager_OnFightCompleted(BossFight bossFigth)
    {
        switch (bossFigth.Result)
        {
            case BossFight.BossFightResult.Draw:
                TryPlaying(Draw);
                break;
            case BossFight.BossFightResult.PlayerWin:
                if (bossFigth.PlayerWins >= 3)
                {
                    TryPlaying(WinCritical);
                }
                else
                {
                    TryPlaying(Win);
                }
                break;
            case BossFight.BossFightResult.PlayerLose:
                TryPlaying(Lose);
                break;
            case BossFight.BossFightResult.Undefined:
            default:
                break;
        }
    }

    private void DeskManager_OnCardMoving()
    {
        TryPlaying(CardMoved);
    }

    private void TryPlaying(string musicKey)
    {
        var availableChannel = GetAvailableChannel();
        if (availableChannel == null)
        {
            GD.Print("No available audio channels");
            return;
        }

        PlayOnChannel(musicKey, availableChannel);
    }

    private AudioStreamPlayer? GetAvailableChannel()
    {
        return musicPlayers.Where(p => !p.Playing).FirstOrDefault();
    }

    private void PlayOnChannel(string musicKey, AudioStreamPlayer availableChannel)
    {
        availableChannel.Stream = Musics[musicKey];
        availableChannel.Play();
    }
}
