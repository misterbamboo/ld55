using Godot;
using System;

public partial class FightSceneUI : Node2D
{
    private static Vector2 FarAway = new Vector2(5000, 5000);

    private event Action OnFadeInEnd;
    private event Action OnTackleEnd;
    private event Action OnFadeOutEnd;

    [Export] private MonsterCardUI playerCard;
    [Export] private MonsterCardUI enemyCard;
    [Export] private Node2D middleHitPoint;

    private GameDataService gameDataService;
    private DeskManager deskManager;

    private BossFightFactory bossFightFactory;

    // Initial positions
    private Vector2 playerInitPos;
    private Vector2 enemyInitPos;

    // Phase1: FadeIn
    private bool fadeIn;
    private double fadeInT;
    private double fadeInSpeed = 3;

    // Phase2: Tackle
    private bool tackleAnim;
    private double tackleAnimT;
    private double tackleAnimRotation = -15;
    private bool tackleHitPending;
    private float tacklePlayerRemainingLife;
    private float tackleEnemyRemainingLife;

    private int tackleRemaining;

    // Phase3: FadeOut
    private bool fadeOut;
    private double fadeOutT;
    private double fadeOutSpeed = 2;

    private BossFight bossFight;

    public override void _Ready()
    {
        gameDataService = GetNode<GameDataService>(GameDataService.Path);
        bossFightFactory = new BossFightFactory(gameDataService);
        deskManager = GetNode<DeskManager>(DeskManager.Path);
        deskManager.OnFightStarting += DeskManager_OnFightStarting;

        playerInitPos = playerCard.Position;
        enemyInitPos = enemyCard.Position;

        OnFadeInEnd += FightSceneUI_OnFadeInEnd;
        OnTackleEnd += FightSceneUI_OnTackleEnd;
        OnFadeOutEnd += FightSceneUI_OnFadeOutEnd;

        Position = FarAway;
        ChangeTransparency(0);
    }

    private void DeskManager_OnFightStarting(SummoningSpecs player, SummoningSpecs enemy)
    {
        StartFight(player, enemy);
    }

    private void FightSceneUI_OnFadeInEnd()
    {
        Phase2_TriggerTackle();
    }

    private void FightSceneUI_OnTackleEnd()
    {
        if (tackleRemaining > 0)
        {
            Phase2_TriggerTackle();
        }
        else
        {
            Phase3_DeclareWinner();
        }
    }

    private void FightSceneUI_OnFadeOutEnd()
    {
        deskManager.FightCompleted(bossFight);
        Position = FarAway;
    }

    public void StartFight(SummoningSpecs player, SummoningSpecs enemy)
    {
        Position = new Vector2(0, 0);
        bossFight = bossFightFactory.Combat(player, enemy);

        tackleRemaining = 3;
        playerCard.Init(player, "player");
        enemyCard.Init(enemy, "enemy");

        Phase1_TriggerFadeIn();
    }

    private void Phase1_TriggerFadeIn()
    {
        fadeInT = 0;
        fadeIn = true;
    }

    private void Phase2_TriggerTackle()
    {
        var player1stTackleHit = bossFight.PlayerLoses > 0 ? 2f : 3f;
        var player2ndTackleHit = bossFight.PlayerLoses > 1 ? player1stTackleHit - 1f : player1stTackleHit;
        var player3rdTackleHit = bossFight.Result == BossFight.BossFightResult.PlayerLose ? 0 : player2ndTackleHit;

        var enemyLoses = bossFight.PlayerWins;
        var enemy1stTackleHit = enemyLoses > 0 ? 2f : 3f;
        var enemy2ndTackleHit = enemyLoses > 1 ? enemy1stTackleHit - 1f : enemy1stTackleHit;
        var enemy3rdTackleHit = bossFight.Result == BossFight.BossFightResult.PlayerWin ? 0 : enemy2ndTackleHit;

        if (tackleRemaining >= 3)
        {
            tacklePlayerRemainingLife = player1stTackleHit;
            tackleEnemyRemainingLife = enemy1stTackleHit;
            tackleHitPending = true;
        }
        else if (tackleRemaining == 2)
        {
            tacklePlayerRemainingLife = player2ndTackleHit;
            tackleEnemyRemainingLife = enemy2ndTackleHit;
            tackleHitPending = true;
        }
        else if (tackleRemaining == 1)
        {
            tacklePlayerRemainingLife = player3rdTackleHit;
            tackleEnemyRemainingLife = enemy3rdTackleHit;
            tackleHitPending = true;
        }

        tackleRemaining--;
        tackleAnimT = 0;
        tackleAnim = true;
    }

    private void Phase3_DeclareWinner()
    {
        fadeOutT = 0;
        fadeOut = true;
    }

    private void ChangeTransparency(float value)
    {
        value = Mathf.Clamp(value, 0f, 1f);
        var mod = Modulate;
        mod.A = value;
        Modulate = mod;
    }

    public override void _Process(double delta)
    {
        AnimFadeIn(delta);
        AnimTackle(delta);
        AnimFadeOut(delta);
    }

    private void AnimFadeIn(double delta)
    {
        if (fadeIn)
        {
            fadeInT += delta * fadeInSpeed;
            ChangeTransparency((float)fadeInT);
            if (fadeInT > 1)
            {
                fadeIn = false;
                OnFadeInEnd?.Invoke();
            }
        }
    }

    private void AnimFadeOut(double delta)
    {
        if (fadeOut)
        {
            fadeOutT += delta * fadeOutSpeed;
            ChangeTransparency(1f - (float)fadeOutT);
            if (fadeOutT > 1)
            {
                fadeOut = false;
                OnFadeOutEnd?.Invoke();
            }
        }
    }

    private void AnimTackle(double delta)
    {
        if (tackleAnim)
        {
            tackleAnimT += delta;
            var t = Ease.Spike(tackleAnimT);
            var tf = (float)t;
            playerCard.Position = playerInitPos.Lerp(middleHitPoint.Position, tf);
            enemyCard.Position = enemyInitPos.Lerp(middleHitPoint.Position, tf);
            playerCard.RotationDegrees = (float)Mathf.Lerp(0, tackleAnimRotation, t * t);
            enemyCard.RotationDegrees = -playerCard.RotationDegrees;

            if (tackleHitPending && tackleAnimT > 0.5)
            {
                tackleHitPending = false;
                playerCard.TriggerChangeProgress(tacklePlayerRemainingLife / 3f);
                enemyCard.TriggerChangeProgress(tackleEnemyRemainingLife / 3f);
                deskManager.FightHit();
            }

            if (tackleAnimT > 1)
            {
                tackleAnimT = 0;
                tackleAnim = false;
                OnTackleEnd?.Invoke();
            }
        }
    }
}
