using System;
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
    }

    private void DeskManager_OnGameStart()
    {
        TryPlaying(GameOver);
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
