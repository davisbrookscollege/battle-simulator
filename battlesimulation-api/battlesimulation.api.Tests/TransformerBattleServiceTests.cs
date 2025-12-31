using System;
using System.Collections.Generic;
using System.Linq;
using battlesimulation.api.Interfaces;
using battlesimulation.api.Models;
using battlesimulation.api.Services;
using Moq;
using Xunit;

public class BattleServiceTests
{
    private void AssertSetEqual(IEnumerable<int> expected, IEnumerable<int> actual)
    {
        Assert.True(expected.ToHashSet().SetEquals(actual));
    }

    private void AssertBattleRecord(Transformer transformer, int wins, int losses)
    {
        Assert.Equal(wins, transformer.Wins);
        Assert.Equal(losses, transformer.Losses);
    }

    private void MockServiceSetUpTransformers(Mock<IFighterService> mockService, IEnumerable<Transformer> transformers)
    {
        foreach (var transformer in transformers)
        {
            mockService.Setup(s => s.Get<Transformer>(transformer.Id))
                       .Returns(transformer);
        }
    }

    [Fact]
    public void Battle_1v1_AutobotsWin_ReturnsCorrectResults()
    {
        var mockService = new Mock<IFighterService>();

        var optimus = new Transformer("Optimus Prime", Faction.Autobot, "Semi-Truck", strength: 10, intelligence: 10);
        optimus.AssignId(1);

        var megatron = new Transformer("Megatron", Faction.Decepticon, "Fusion Cannon", strength: 9, intelligence: 9);
        megatron.AssignId(2);

        MockServiceSetUpTransformers(mockService, new List<Transformer> { optimus, megatron });

        var battleService = new BattleService(mockService.Object);

        var results = battleService.Battle<Transformer>(new List<int> { 1, 2 });

        Assert.False(results.IsTie);

        AssertBattleRecord(optimus, 1, 0);
        AssertBattleRecord(megatron, 0, 1);

        AssertSetEqual(new[] { 1 }, results.Winners);
        AssertSetEqual(new[] { 2 }, results.Losers);
    }

    [Fact]
    public void Battle_1v1_DecepticonsWin_ReturnsCorrectResults()
    {
        var mockService = new Mock<IFighterService>();

        var jazz = new Transformer("Jazz", Faction.Autobot, "Porsche 935", strength: 7, intelligence: 8);
        jazz.AssignId(1);

        var shockwave = new Transformer("Shockwave", Faction.Decepticon, "Laser Cannon", strength: 10, intelligence: 7);
        shockwave.AssignId(2);

        MockServiceSetUpTransformers(mockService, new List<Transformer> { jazz, shockwave });

        var battleService = new BattleService(mockService.Object);

        var results = battleService.Battle<Transformer>(new List<int> { 1, 2 });

        Assert.False(results.IsTie);

        AssertBattleRecord(jazz, 0, 1);
        AssertBattleRecord(shockwave, 1, 0);

        AssertSetEqual(new[] { 2 }, results.Winners);
        AssertSetEqual(new[] { 1 }, results.Losers);
    }

    [Fact]
    public void Battle_NoFighters_ReturnsTieWithNoResults()
    {
        var mockService = new Mock<IFighterService>();

        var battleService = new BattleService(mockService.Object);

        var results = battleService.Battle<Transformer>(new List<int>());

        Assert.True(results.IsTie);
        Assert.Empty(results.Winners);
        Assert.Empty(results.Losers);
    }

    [Fact]
    public void Battle_4Fighters_Tie()
    {
        var mockService = new Mock<IFighterService>();

        var optimus = new Transformer("Optimus Prime", Faction.Autobot, "Semi-Truck", 10, 5);
        optimus.AssignId(1);

        var bumblebee = new Transformer("Bumblebee", Faction.Autobot, "Camaro", 6, 5);
        bumblebee.AssignId(2);

        var starscream = new Transformer("Starscream", Faction.Decepticon, "F-15 Jet", 8, 5);
        starscream.AssignId(3);

        var soundwave = new Transformer("Soundwave", Faction.Decepticon, "Hovercraft", 8, 5);
        soundwave.AssignId(4);

        MockServiceSetUpTransformers(
            mockService,
            new List<Transformer> { optimus, bumblebee, starscream, soundwave }
        );

        var battleService = new BattleService(mockService.Object);

        var results = battleService.Battle<Transformer>(new List<int> { 1, 2, 3, 4 });

        Assert.True(results.IsTie);

        AssertBattleRecord(optimus, 0, 1);
        AssertBattleRecord(bumblebee, 0, 1);
        AssertBattleRecord(starscream, 0, 1);
        AssertBattleRecord(soundwave, 0, 1);

        AssertSetEqual(Array.Empty<int>(), results.Winners);
        AssertSetEqual(new[] { 1, 2, 3, 4 }, results.Losers);
    }

    [Fact]
    public void Battle_OneSided_AutobotsOnly_AutobotsWin()
    {
        var mockService = new Mock<IFighterService>();

        var ironhide = new Transformer("Ironhide", Faction.Autobot, "GMC Topkick", strength: 9, intelligence: 8);
        ironhide.AssignId(1);

        MockServiceSetUpTransformers(mockService, new List<Transformer> { ironhide });

        var battleService = new BattleService(mockService.Object);

        var results = battleService.Battle<Transformer>(new List<int> { 1 });

        Assert.False(results.IsTie);

        AssertBattleRecord(ironhide, 1, 0);

        AssertSetEqual(new[] { 1 }, results.Winners);
        AssertSetEqual(Array.Empty<int>(), results.Losers);
    }

    [Fact]
    public void Battle_OneSided_DecepticonsOnly_DecepticonsWin()
    {
        var mockService = new Mock<IFighterService>();

        var soundwave = new Transformer("Soundwave", Faction.Decepticon, "Cybertronian Hovercraft", strength: 10, intelligence: 9);
        soundwave.AssignId(1);

        MockServiceSetUpTransformers(mockService, new List<Transformer> { soundwave });

        var battleService = new BattleService(mockService.Object);

        var results = battleService.Battle<Transformer>(new List<int> { 1 });

        Assert.False(results.IsTie);

        AssertBattleRecord(soundwave, 1, 0);

        AssertSetEqual(new[] { 1 }, results.Winners);
        AssertSetEqual(Array.Empty<int>(), results.Losers);
    }

    [Fact]
    public void Battle_3Fighters_AutobotsWin()
    {
        var mockService = new Mock<IFighterService>();

        var optimus = new Transformer("Optimus Prime", Faction.Autobot, "Semi-Truck", 10, 10);
        optimus.AssignId(1);

        var bumblebee = new Transformer("Bumblebee", Faction.Autobot, "Camaro", 6, 7);
        bumblebee.AssignId(2);

        var starscream = new Transformer("Starscream", Faction.Decepticon, "F-15 Jet", 8, 8);
        starscream.AssignId(3);

        MockServiceSetUpTransformers(mockService, new List<Transformer> { optimus, bumblebee, starscream });

        var battleService = new BattleService(mockService.Object);

        var results = battleService.Battle<Transformer>(new List<int> { 1, 2, 3 });

        Assert.False(results.IsTie);

        AssertBattleRecord(optimus, 1, 0);
        AssertBattleRecord(bumblebee, 1, 0);
        AssertBattleRecord(starscream, 0, 1);

        AssertSetEqual(new[] { 1, 2 }, results.Winners);
        AssertSetEqual(new[] { 3 }, results.Losers);
    }

    [Fact]
    public void Battle_3Fighters_DecepticonsWin()
    {
        var mockService = new Mock<IFighterService>();

        var hotrod = new Transformer("Hot Rod", Faction.Autobot, "Sports Car", 7, 6);
        hotrod.AssignId(1);

        var cliffjumper = new Transformer("Cliffjumper", Faction.Autobot, "Compact Car", 6, 5);
        cliffjumper.AssignId(2);

        var megatron = new Transformer("Megatron", Faction.Decepticon, "Fusion Cannon", 10, 9);
        megatron.AssignId(3);

        MockServiceSetUpTransformers(mockService, new List<Transformer> { hotrod, cliffjumper, megatron });

        var battleService = new BattleService(mockService.Object);

        var results = battleService.Battle<Transformer>(new List<int> { 1, 2, 3 });

        Assert.False(results.IsTie);

        AssertBattleRecord(hotrod, 0, 1);
        AssertBattleRecord(cliffjumper, 0, 1);
        AssertBattleRecord(megatron, 1, 0);

        AssertSetEqual(new[] { 3 }, results.Winners);
        AssertSetEqual(new[] { 1, 2 }, results.Losers);
    }

    [Fact]
    public void Battle_4Fighters_DecepticonsWin()
    {
        var mockService = new Mock<IFighterService>();

        var ironhide = new Transformer("Ironhide", Faction.Autobot, "GMC Topkick", 9, 8);
        ironhide.AssignId(1);

        var ratchet = new Transformer("Ratchet", Faction.Autobot, "Ambulance", 7, 9);
        ratchet.AssignId(2);

        var soundwave = new Transformer("Soundwave", Faction.Decepticon, "Hovercraft", 10, 9);
        soundwave.AssignId(3);

        var shockwave = new Transformer("Shockwave", Faction.Decepticon, "Laser Cannon", 10, 10);
        shockwave.AssignId(4);

        MockServiceSetUpTransformers(mockService, new List<Transformer> { ironhide, ratchet, soundwave, shockwave });

        var battleService = new BattleService(mockService.Object);

        var results = battleService.Battle<Transformer>(new List<int> { 1, 2, 3, 4 });

        Assert.False(results.IsTie);

        AssertBattleRecord(ironhide, 0, 1);
        AssertBattleRecord(ratchet, 0, 1);
        AssertBattleRecord(soundwave, 1, 0);
        AssertBattleRecord(shockwave, 1, 0);

        AssertSetEqual(new[] { 3, 4 }, results.Winners);
        AssertSetEqual(new[] { 1, 2 }, results.Losers);
    }

    [Fact]
    public void Battle_4Fighters_AutobotsWin()
    {
        var mockService = new Mock<IFighterService>();

        var optimus = new Transformer("Optimus Prime", Faction.Autobot, "Semi-Truck", 10, 10);
        optimus.AssignId(1);

        var ironhide = new Transformer("Ironhide", Faction.Autobot, "GMC Topkick", 9, 8);
        ironhide.AssignId(2);

        var soundwave = new Transformer("Soundwave", Faction.Decepticon, "Hovercraft", 8, 7);
        soundwave.AssignId(3);

        var thundercracker = new Transformer("Thundercracker", Faction.Decepticon, "F-15 Jet", 6, 5);
        thundercracker.AssignId(4);

        MockServiceSetUpTransformers(
            mockService,
            new List<Transformer> { optimus, ironhide, soundwave, thundercracker }
        );

        var battleService = new BattleService(mockService.Object);

        var results = battleService.Battle<Transformer>(new List<int> { 1, 2, 3, 4 });

        Assert.False(results.IsTie);

        AssertBattleRecord(optimus, 1, 0);
        AssertBattleRecord(ironhide, 1, 0);
        AssertBattleRecord(soundwave, 0, 1);
        AssertBattleRecord(thundercracker, 0, 1);

        AssertSetEqual(new[] { 1, 2 }, results.Winners);
        AssertSetEqual(new[] { 3, 4 }, results.Losers);
    }

    [Fact]
    public void Battle_7Fighters_AutobotsWin()
    {
        var mockService = new Mock<IFighterService>();

        var optimus = new Transformer("Optimus Prime", Faction.Autobot, "Semi-Truck", 10, 10); optimus.AssignId(1);
        var ironhide = new Transformer("Ironhide", Faction.Autobot, "GMC Topkick", 9, 8); ironhide.AssignId(2);
        var ratchet = new Transformer("Ratchet", Faction.Autobot, "Ambulance", 7, 9); ratchet.AssignId(3);

        var starscream = new Transformer("Starscream", Faction.Decepticon, "F-15 Jet", 7, 7); starscream.AssignId(4);
        var thundercracker = new Transformer("Thundercracker", Faction.Decepticon, "F-15 Jet", 6, 5); thundercracker.AssignId(5);
        var skywarp = new Transformer("Skywarp", Faction.Decepticon, "F-15 Jet", 7, 6); skywarp.AssignId(6);
        var blitzwing = new Transformer("Blitzwing", Faction.Decepticon, "Triple Changer", 8, 7); blitzwing.AssignId(7);

        var all = new List<Transformer>
        {
            optimus, ironhide, ratchet,
            starscream, thundercracker, skywarp, blitzwing
        };

        MockServiceSetUpTransformers(mockService, all);

        var battleService = new BattleService(mockService.Object);

        var results = battleService.Battle<Transformer>(new List<int> { 1, 2, 3, 4, 5, 6, 7 });

        Assert.False(results.IsTie);

        AssertBattleRecord(optimus, 1, 0);
        AssertBattleRecord(ironhide, 1, 0);
        AssertBattleRecord(ratchet, 1, 0);

        AssertBattleRecord(starscream, 0, 1);
        AssertBattleRecord(thundercracker, 0, 1);
        AssertBattleRecord(skywarp, 0, 1);
        AssertBattleRecord(blitzwing, 0, 1);

        AssertSetEqual(new[] { 1, 2, 3 }, results.Winners);
        AssertSetEqual(new[] { 4, 5, 6, 7 }, results.Losers);
    }

    [Fact]
    public void Battle_7Fighters_DecepticonsWin()
    {
        var mockService = new Mock<IFighterService>();

        var hotrod = new Transformer("Hot Rod", Faction.Autobot, "Sports Car", 7, 6); hotrod.AssignId(1);
        var jazz = new Transformer("Jazz", Faction.Autobot, "Porsche 935", 7, 8); jazz.AssignId(2);
        var bumblebee = new Transformer("Bumblebee", Faction.Autobot, "Camaro", 6, 7); bumblebee.AssignId(3);

        var megatron = new Transformer("Megatron", Faction.Decepticon, "Fusion Cannon", 10, 9); megatron.AssignId(4);
        var shockwave = new Transformer("Shockwave", Faction.Decepticon, "Laser Cannon", 10, 10); shockwave.AssignId(5);
        var soundwave = new Transformer("Soundwave", Faction.Decepticon, "Hovercraft", 9, 9); soundwave.AssignId(6);
        var blitzwing = new Transformer("Blitzwing", Faction.Decepticon, "Triple Changer", 9, 8); blitzwing.AssignId(7);

        var all = new List<Transformer>
        {
            hotrod, jazz, bumblebee,
            megatron, shockwave, soundwave, blitzwing
        };

        MockServiceSetUpTransformers(mockService, all);

        var battleService = new BattleService(mockService.Object);

        var results = battleService.Battle<Transformer>(new List<int> { 1, 2, 3, 4, 5, 6, 7 });

        Assert.False(results.IsTie);

        AssertBattleRecord(hotrod, 0, 1);
        AssertBattleRecord(jazz, 0, 1);
        AssertBattleRecord(bumblebee, 0, 1);

        AssertBattleRecord(megatron, 1, 0);
        AssertBattleRecord(shockwave, 1, 0);
        AssertBattleRecord(soundwave, 1, 0);
        AssertBattleRecord(blitzwing, 1, 0);

        AssertSetEqual(new[] { 4, 5, 6, 7 }, results.Winners);
        AssertSetEqual(new[] { 1, 2, 3 }, results.Losers);
    }

    [Fact]
    public void Battle_4Fighters_MixedNegativeValues_AutobotsWin()
    {
        var mockService = new Mock<IFighterService>();

        var a1 = new Transformer("A1", Faction.Autobot, "Car", 8, 7); a1.AssignId(1);
        var a2 = new Transformer("A2", Faction.Autobot, "Car", 5, -1); a2.AssignId(2);

        var d1 = new Transformer("D1", Faction.Decepticon, "Jet", 3, -2); d1.AssignId(3);
        var d2 = new Transformer("D2", Faction.Decepticon, "Jet", -1, 4); d2.AssignId(4);

        MockServiceSetUpTransformers(mockService, new List<Transformer> { a1, a2, d1, d2 });

        var battleService = new BattleService(mockService.Object);

        var results = battleService.Battle<Transformer>(new List<int> { 1, 2, 3, 4 });

        Assert.False(results.IsTie);

        AssertBattleRecord(a1, 1, 0);
        AssertBattleRecord(a2, 1, 0);
        AssertBattleRecord(d1, 0, 1);
        AssertBattleRecord(d2, 0, 1);

        AssertSetEqual(new[] { 1, 2 }, results.Winners);
        AssertSetEqual(new[] { 3, 4 }, results.Losers);
    }

    [Fact]
    public void Battle_4Fighters_AllNegativeValues_DecepticonsWin()
    {
        var mockService = new Mock<IFighterService>();

        var a1 = new Transformer("A1", Faction.Autobot, "Car", -3, -2); a1.AssignId(1);
        var a2 = new Transformer("A2", Faction.Autobot, "Car", -2, -1); a2.AssignId(2);

        var d1 = new Transformer("D1", Faction.Decepticon, "Jet", -5, -5); d1.AssignId(3);
        var d2 = new Transformer("D2", Faction.Decepticon, "Jet", -4, -4); d2.AssignId(4);

        MockServiceSetUpTransformers(mockService, new List<Transformer> { a1, a2, d1, d2 });

        var battleService = new BattleService(mockService.Object);

        var results = battleService.Battle<Transformer>(new List<int> { 1, 2, 3, 4 });

        Assert.False(results.IsTie);

        AssertBattleRecord(a1, 0, 1);
        AssertBattleRecord(a2, 0, 1);
        AssertBattleRecord(d1, 1, 0);
        AssertBattleRecord(d2, 1, 0);

        AssertSetEqual(new[] { 3, 4 }, results.Winners);
        AssertSetEqual(new[] { 1, 2 }, results.Losers);
    }

    [Fact]
    public void Battle_4Fighters_MixedZeroValues_DecepticonsWin()
    {
        var mockService = new Mock<IFighterService>();

        var a1 = new Transformer("A1", Faction.Autobot, "Car", 3, 5); a1.AssignId(1);
        var a2 = new Transformer("A2", Faction.Autobot, "Car", 3, 0); a2.AssignId(2);

        var d1 = new Transformer("D1", Faction.Decepticon, "Jet", 0, 3); d1.AssignId(3);
        var d2 = new Transformer("D2", Faction.Decepticon, "Jet", 5, 9); d2.AssignId(4);

        MockServiceSetUpTransformers(mockService, new List<Transformer> { a1, a2, d1, d2 });

        var battleService = new BattleService(mockService.Object);

        var results = battleService.Battle<Transformer>(new List<int> { 1, 2, 3, 4 });

        Assert.False(results.IsTie);

        AssertBattleRecord(a1, 0, 1);
        AssertBattleRecord(a2, 0, 1);
        AssertBattleRecord(d1, 1, 0);
        AssertBattleRecord(d2, 1, 0);

        AssertSetEqual(new[] { 3, 4 }, results.Winners);
        AssertSetEqual(new[] { 1, 2 }, results.Losers);
    }

    [Fact]
    public void Battle_4Fighters_AllZeroValues_Tie()
    {
        var mockService = new Mock<IFighterService>();

        var a1 = new Transformer("A1", Faction.Autobot, "Car", 0, 0); a1.AssignId(1);
        var a2 = new Transformer("A2", Faction.Autobot, "Car", 0, 0); a2.AssignId(2);

        var d1 = new Transformer("D1", Faction.Decepticon, "Jet", 0, 0); d1.AssignId(3);
        var d2 = new Transformer("D2", Faction.Decepticon, "Jet", 0, 0); d2.AssignId(4);

        MockServiceSetUpTransformers(mockService, new List<Transformer> { a1, a2, d1, d2 });

        var battleService = new BattleService(mockService.Object);

        var results = battleService.Battle<Transformer>(new List<int> { 1, 2, 3, 4 });

        Assert.True(results.IsTie);

        AssertBattleRecord(a1, 0, 1);
        AssertBattleRecord(a2, 0, 1);
        AssertBattleRecord(d1, 0, 1);
        AssertBattleRecord(d2, 0, 1);

        AssertSetEqual(Array.Empty<int>(), results.Winners);
        AssertSetEqual(new[] { 1, 2, 3, 4 }, results.Losers);
    }

    [Fact]
    public void Battle_RogueFaction_DecepticonsWin()
    {
        var mockService = new Mock<IFighterService>();

        var a1 = new Transformer("A1", Faction.Autobot, "Car", 5, 5); a1.AssignId(1);
        var a2 = new Transformer("A2", Faction.Autobot, "Car", 7, 7); a2.AssignId(2);

        var d1 = new Transformer("D1", Faction.Decepticon, "Jet", 5, 5); d1.AssignId(3);
        var d2 = new Transformer("D2", Faction.Decepticon, "Jet", 7, 7); d2.AssignId(4);

        var r1 = new Transformer("R1", Faction.Removed, "Oscar Mayer Wienermobile", 2, 2); r1.AssignId(5);

        MockServiceSetUpTransformers(mockService, new List<Transformer> { a1, a2, d1, d2, r1 });

        var battleService = new BattleService(mockService.Object);

        var results = battleService.Battle<Transformer>(new List<int> { 1, 2, 3, 4, 5 });

        Assert.False(results.IsTie);

        AssertBattleRecord(a1, 0, 1);
        AssertBattleRecord(a2, 0, 1);
        AssertBattleRecord(d1, 1, 0);
        AssertBattleRecord(d2, 1, 0);
        AssertBattleRecord(r1, 1, 0);

        AssertSetEqual(new[] { 3, 4, 5 }, results.Winners);
        AssertSetEqual(new[] { 1, 2 }, results.Losers);
    }

    [Fact]
    public void Battle_TwoStage_4Then6Fighters()
    {
        var mockService = new Mock<IFighterService>();

        var a1 = new Transformer("A1", Faction.Autobot, "Car", 9, 9); a1.AssignId(1);
        var a2 = new Transformer("A2", Faction.Autobot, "Car", 7, 7); a2.AssignId(2);

        var d1 = new Transformer("D1", Faction.Decepticon, "Jet", 6, 6); d1.AssignId(3);
        var d2 = new Transformer("D2", Faction.Decepticon, "Jet", 5, 5); d2.AssignId(4);

        var a3 = new Transformer("A3", Faction.Autobot, "Car", 4, 4); a3.AssignId(5);
        var d3 = new Transformer("D3", Faction.Decepticon, "Jet", 3, 3); d3.AssignId(6);

        var all = new List<Transformer> { a1, a2, d1, d2, a3, d3 };

        MockServiceSetUpTransformers(mockService, all);

        var battleService = new BattleService(mockService.Object);

        var firstBattle = battleService.Battle<Transformer>(new List<int> { 1, 2, 3, 4 });

        Assert.False(firstBattle.IsTie);

        AssertBattleRecord(a1, 1, 0);
        AssertBattleRecord(a2, 1, 0);
        AssertBattleRecord(d1, 0, 1);
        AssertBattleRecord(d2, 0, 1);

        AssertSetEqual(new[] { 1, 2 }, firstBattle.Winners);
        AssertSetEqual(new[] { 3, 4 }, firstBattle.Losers);

        var secondBattle = battleService.Battle<Transformer>(new List<int> { 1, 2, 3, 4, 5, 6 });

        Assert.False(secondBattle.IsTie);

        AssertBattleRecord(a1, 2, 0);
        AssertBattleRecord(a2, 2, 0);
        AssertBattleRecord(a3, 1, 0);

        AssertBattleRecord(d1, 0, 2);
        AssertBattleRecord(d2, 0, 2);
        AssertBattleRecord(d3, 0, 1);

        AssertSetEqual(new[] { 1, 2, 5 }, secondBattle.Winners);
        AssertSetEqual(new[] { 3, 4, 6 }, secondBattle.Losers);
    }

    [Fact]
    public void Battle_ThreeStage_ManyWinAndLoseAtLeastOnce()
    {
        var mockService = new Mock<IFighterService>();

        var a1 = new Transformer("A1", Faction.Autobot, "Car", 9, 9); a1.AssignId(1);
        var a2 = new Transformer("A2", Faction.Autobot, "Car", 7, 7); a2.AssignId(2);
        var a3 = new Transformer("A3", Faction.Autobot, "Car", 6, 6); a3.AssignId(3);

        var d1 = new Transformer("D1", Faction.Decepticon, "Jet", 10, 3); d1.AssignId(4);
        var d2 = new Transformer("D2", Faction.Decepticon, "Jet", 8, 4); d2.AssignId(5);
        var d3 = new Transformer("D3", Faction.Decepticon, "Jet", 5, 5); d3.AssignId(6);

        var all = new List<Transformer> { a1, a2, a3, d1, d2, d3 };

        MockServiceSetUpTransformers(mockService, all);

        var battleService = new BattleService(mockService.Object);

        var stage1 = battleService.Battle<Transformer>(new List<int> { 1, 4, 5 });

        Assert.False(stage1.IsTie);

        AssertBattleRecord(a1, 1, 0);
        AssertBattleRecord(d1, 0, 1);
        AssertBattleRecord(d2, 0, 1);

        AssertSetEqual(new[] { 1 }, stage1.Winners);
        AssertSetEqual(new[] { 4, 5 }, stage1.Losers);

        var stage2 = battleService.Battle<Transformer>(new List<int> { 2, 3, 4, 6 });

        Assert.False(stage2.IsTie);

        AssertBattleRecord(a2, 1, 0);
        AssertBattleRecord(a3, 1, 0);
        AssertBattleRecord(d1, 0, 2);
        AssertBattleRecord(d3, 0, 1);

        AssertSetEqual(new[] { 2, 3 }, stage2.Winners);
        AssertSetEqual(new[] { 4, 6 }, stage2.Losers);

        var stage3 = battleService.Battle<Transformer>(new List<int> { 2, 3, 4, 5, 6 });

        Assert.False(stage3.IsTie);

        AssertBattleRecord(a2, 1, 1);
        AssertBattleRecord(a3, 1, 1);

        AssertBattleRecord(d1, 1, 2);
        AssertBattleRecord(d2, 1, 1);
        AssertBattleRecord(d3, 1, 1);

        AssertSetEqual(new[] { 4, 5, 6 }, stage3.Winners);
        AssertSetEqual(new[] { 2, 3 }, stage3.Losers);
    }

    [Fact]
    public void Battle_4Fighters_MixedUpIdOrder_AutobotsWin()
    {
        var mockService = new Mock<IFighterService>();

        var a1 = new Transformer("A1", Faction.Autobot, "Car", 9, 9); a1.AssignId(1);
        var a2 = new Transformer("A2", Faction.Autobot, "Car", 7, 7); a2.AssignId(2);

        var d1 = new Transformer("D1", Faction.Decepticon, "Jet", 5, 5); d1.AssignId(3);
        var d2 = new Transformer("D2", Faction.Decepticon, "Jet", 4, 4); d2.AssignId(4);

        MockServiceSetUpTransformers(mockService, new List<Transformer> { a1, a2, d1, d2 });

        var battleService = new BattleService(mockService.Object);

        var results = battleService.Battle<Transformer>(new List<int> { 4, 1, 3, 2 });

        Assert.False(results.IsTie);

        AssertBattleRecord(a1, 1, 0);
        AssertBattleRecord(a2, 1, 0);
        AssertBattleRecord(d1, 0, 1);
        AssertBattleRecord(d2, 0, 1);

        AssertSetEqual(new[] { 1, 2 }, results.Winners);
        AssertSetEqual(new[] { 3, 4 }, results.Losers);
    }

    [Fact]
    public void Battle_4Fighters_DuplicateIds_DecepticonsWin()
    {
        var mockService = new Mock<IFighterService>();

        var a1 = new Transformer("A1", Faction.Autobot, "Car", 5, 5); a1.AssignId(1);
        var a2 = new Transformer("A2", Faction.Autobot, "Car", 4, 4); a2.AssignId(2);

        var d1 = new Transformer("D1", Faction.Decepticon, "Jet", 9, 9); d1.AssignId(3);
        var d2 = new Transformer("D2", Faction.Decepticon, "Jet", 8, 8); d2.AssignId(4);

        MockServiceSetUpTransformers(mockService, new List<Transformer> { a1, a2, d1, d2 });

        var battleService = new BattleService(mockService.Object);

        var results = battleService.Battle<Transformer>(new List<int> { 1, 3, 3, 4, 2, 4, 2 });

        Assert.False(results.IsTie);

        AssertBattleRecord(a1, 0, 1);
        AssertBattleRecord(a2, 0, 2);
        AssertBattleRecord(d1, 2, 0);
        AssertBattleRecord(d2, 2, 0);

        AssertSetEqual(new[] { 3, 3, 4, 4 }, results.Winners);
        AssertSetEqual(new[] { 1, 2, 2 }, results.Losers);
    }
}
