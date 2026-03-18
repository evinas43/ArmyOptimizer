    using System.Collections.ObjectModel;
    using System.Linq;
using System.Windows;
using ArmyOptimizer.Models;
using ArmyOptimizer.Services;
using ArmyOptimizer.Utilities;

    namespace ArmyOptimizer.ViewModels
    {
        public class OptimizeArmyVM : ViewModelBase
        {
            private readonly NavigationVM _navigation;

            
            //Back home and go to save army commands
            public RelayCommand BackCommand { get; }
            public RelayCommand SaveArmyCommand { get; }
            //------------------------------------------

            //Toggle selection commands for troops,spells and siege machines
            public RelayCommand<SelectableTroop> ToggleTroopCommand { get; }
            public RelayCommand<SelectableSpell> ToggleSpellCommand { get; }
            public RelayCommand<SelectableSiegeMachine> ToggleSiegeMachinesCommand { get; }
            public RelayCommand OptimizeCommand { get; }
            //-----------------------------------------------------------------
            

            //Optimizer army result , this variable will contain the optimized army to pass it to the save armyVM in case the user wants to save it 
            public AiArmyResponse OptimizedResult { get; set; }
            //-------------------------


            
            //navigation inside the optimizer stepup
            public RelayCommand NextCommand { get; }
            public RelayCommand PreviousStepCommand { get; }
            //------------------------------------------

            //housing properties for Troops, Spells and Siege Machines
            public bool IsArmyFull => CurrentHousing >= MaxHousing;
            public bool IsSpellFull => CurrentSpellHousing >= MaxSpellHousing;
            public bool IsSiegeFull => CurrentSiegeMachineHousing >= MaxSiegeMachineHousing;
            //------------------------------------------------------------------------------
        

            
            public RelayCommand<int> SelectTownHallCommand { get; }
            public bool HasSelectedHero => Heroes?.Any(h => h.IsSelected) == true;
            public bool HasSelectedSpell =>ElixirSpells.Any(s => s.Quantity > 0 ) ||DarkSpells.Any(s => s.Quantity > 0 );
            public bool HasSelectedSiegeMachine => SiegeMachines.Any(s => s.Quantity > 0);
            public int TownHallLevel => ArmyToOptimize.TownHallLevel;


        private int _currentStep = 1;
            public int CurrentStep
            {
                get => _currentStep;
                set
                {
                    _currentStep = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsTownHallStep));
                    OnPropertyChanged(nameof(IsArmyStep));
                    OnPropertyChanged(nameof(IsSiegeMachineStep));
                    OnPropertyChanged(nameof(ShowBackButton));
                    OnPropertyChanged(nameof(IsSpellStep));
                    OnPropertyChanged(nameof(IsSummaryStep));
                    OnPropertyChanged(nameof(IsOptimizedStep));

                }
            }


            public bool ShowBackButton => CurrentStep != 1;
            public bool IsTownHallStep => CurrentStep == 1;
            public bool IsArmyStep => CurrentStep == 2;
            public bool IsSiegeMachineStep => CurrentStep == 3 && ArmyToOptimize.TownHallLevel >= 12;
            public bool IsSpellStep =>(ArmyToOptimize.TownHallLevel >= 12 && CurrentStep == 4) ||(ArmyToOptimize.TownHallLevel < 12 && CurrentStep == 3);
            public bool IsSummaryStep =>(ArmyToOptimize.TownHallLevel >= 12 && CurrentStep == 5)|| (ArmyToOptimize.TownHallLevel < 12 && CurrentStep == 4);
            public bool IsOptimizedStep => (ArmyToOptimize.TownHallLevel >= 12 && CurrentStep == 6) || (ArmyToOptimize.TownHallLevel < 12 && CurrentStep == 5);


            private int _maxHousing;
            public int MaxHousing
            {
                get => _maxHousing;
                set
                {
                    _maxHousing = value;
                    OnPropertyChanged();
                }
            }

            private int _maxSpellHousing;
            public int MaxSpellHousing
            {
                get => _maxSpellHousing;
                set
                {
                    _maxSpellHousing = value;
                    OnPropertyChanged();
                }
            }

            private int _maxSiegeMachineHousing;

            public int MaxSiegeMachineHousing
            {
                get => _maxSiegeMachineHousing;
                set
                {
                    _maxSiegeMachineHousing = value;
                    OnPropertyChanged();
                }
            }


            private int _currentHousing;
            public int CurrentHousing
            {
                get => _currentHousing;
                set
                {
                    _currentHousing = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsArmyFull));

                }
            }
            private int _currentSpellHousing;
            public int CurrentSpellHousing
            {
                get => _currentSpellHousing;
                set
                {
                    _currentSpellHousing = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsSpellFull));
                }
            }

            private int _currentSiegeMachineHousing;
            public int CurrentSiegeMachineHousing
            {
                get => _currentSiegeMachineHousing;
                set
                {
                    _currentSiegeMachineHousing = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsSiegeFull));
            }
            }


            private void UpdateHousing()
            {
                CurrentHousing =
                    (ElixirTroops?.Sum(t => t.TotalHousing) ?? 0) +
                    (DarkTroops?.Sum(t => t.TotalHousing) ?? 0) +
                    (SuperTroops?.Sum(t => t.TotalHousing) ?? 0);
            }
            private void UpdateSpellHousing()
            {
                CurrentSpellHousing =
                    (ElixirSpells?.Sum(s => s.TotalSpellHousing) ?? 0) +
                    (DarkSpells?.Sum(s => s.TotalSpellHousing) ?? 0);
            }

            private void UpdateSiegeMachineHousing()
            {
                CurrentSiegeMachineHousing =(SiegeMachines?.Sum(s => s.Quantity * s.MachineHousing) ?? 0);

            }

        private void BuildArmyModel()
        {
            ArmyToOptimize.SelectedHeroes =
                Heroes.Where(h => h.IsSelected)
                .Select(h => h.Name)
                .ToList();

            ArmyToOptimize.SelectedTroops =
                ElixirTroops
                .Concat(DarkTroops)
                .Concat(SuperTroops)
                .Where(t => t.Quantity > 0)
                .Select(t => new ArmyUnit
                {
                    Name = t.Name,
                    Quantity = t.Quantity
                })
                .ToList();

            ArmyToOptimize.SelectedSpells =
                ElixirSpells
                .Concat(DarkSpells)
                .Where(s => s.Quantity > 0)
                .Select(s => new ArmyUnit
                {
                    Name = s.Name,
                    Quantity = s.Quantity
                })
                .ToList();

            ArmyToOptimize.SelectedSiegeMachines =
                SiegeMachines
                .Where(s => s.Quantity > 0)
                .Select(s => new ArmyUnit
                {
                    Name = s.Name,
                    Quantity = s.Quantity
                })
                .ToList();

            OnPropertyChanged(nameof(SelectedHeroes));
            OnPropertyChanged(nameof(SelectedElixirTroops));
            OnPropertyChanged(nameof(SelectedDarkTroops));
            OnPropertyChanged(nameof(SelectedSuperTroops));
            OnPropertyChanged(nameof(SelectedElixirSpells));
            OnPropertyChanged(nameof(SelectedDarkSpells));
            OnPropertyChanged(nameof(SelectedSiegeMachines));
        }

        private int GetHousingForTownHall(int th)
            {
                return th switch
                {
                    7 => 80,
                    8 => 90,
                    9 => 100,
                    10 => 110,
                    11 => 120,
                    12 => 130,
                    13 => 140,
                    14 => 150,
                    15 => 160,
                    16 => 170,
                    17 => 180,
                    18 => 340,
                    _ => 200
                };
            }
            private int GetSpellCapacityForTownHall(int th)
            {
                return th switch
                {
                    7 => 6,
                    8 => 7,
                    9 => 9,
                    10 => 11,
                    11 => 11,
                    12 => 11,
                    13 => 11,
                    14 => 11,
                    15 => 11,
                    16 => 11,
                    17 => 11,
                    18 => 11,
                    _ => 0
                };
            }

            private int GetSpellsSiegeMachineCapacityForTownHall(int th)
            {
                return th switch
                {
                    7 => 0,
                    8 => 0,
                    9 => 0,
                    10 => 0,
                    11 => 0,
                    12 => 3,
                    13 => 3,
                    14 => 3,
                    15 => 3,
                    16 => 3,
                    17 => 3,
                    18 => 3,
                    _ => 0
                };
            }


            public ArmyToOptimize ArmyToOptimize { get; set; }

            public ObservableCollection<SelectableHero> Heroes { get; set; }
            public ObservableCollection<SelectableTroop> ElixirTroops { get; set; }
            public ObservableCollection<SelectableTroop> DarkTroops { get; set; }
            public ObservableCollection<SelectableTroop> SuperTroops { get; set; }
            public ObservableCollection<SelectableSpell> ElixirSpells { get; set; }
            public ObservableCollection<SelectableSpell> DarkSpells { get; set; }
            public ObservableCollection<SelectableSiegeMachine> SiegeMachines { get; set; }

        //summary properties 
            //heroes
            public IEnumerable<SelectableHero> SelectedHeroes =>Heroes.Where(h => h.IsSelected);

        //Troops,DarkTroops,SuperTroops
            public IEnumerable<SelectableTroop> SelectedElixirTroops =>ElixirTroops.Where(t => t.Quantity > 0);

            public IEnumerable<SelectableTroop> SelectedDarkTroops =>DarkTroops.Where(t => t.Quantity > 0);

            public IEnumerable<SelectableTroop> SelectedSuperTroops =>SuperTroops.Where(t => t.Quantity > 0);

        //SiegeMachines
            public IEnumerable<SelectableSiegeMachine> SelectedSiegeMachines =>SiegeMachines.Where(s => s.Quantity > 0);

        //spells,DarkSpells

            public IEnumerable<SelectableSpell> SelectedElixirSpells =>ElixirSpells.Where(s => s.Quantity > 0);

            public IEnumerable<SelectableSpell> SelectedDarkSpells =>DarkSpells.Where(s => s.Quantity > 0);

            private readonly AiOptimizerService _optimizer;



        //constructor  
        public OptimizeArmyVM(NavigationVM navigation)
            {
                _navigation = navigation;

                _optimizer = new AiOptimizerService(HttpService.Client);

                OptimizeCommand = new RelayCommand(async _ => await Optimize());

            // 1 
            ArmyToOptimize = new ArmyToOptimize();

                // 2 Initialize Data First
                InitializeCollections();

                // 3 Initialize The Quantity selector
                InitializeTroopCommands();

                // 4 Commands
                BackCommand = new RelayCommand(_ =>
                    _navigation.CurrentView = new HomeVM(_navigation));

                PreviousStepCommand = new RelayCommand(_ =>
                {
                    if (CurrentStep == 2)
                        CurrentStep = 1;

                    else if (CurrentStep == 3)
                        CurrentStep = 2;

                    else if (CurrentStep == 4)
                    {
                        if (ArmyToOptimize.TownHallLevel >= 12)
                            CurrentStep = 3;
                        else
                            CurrentStep = 2;
                    }

                    else if (CurrentStep == 5)
                    {
                        if (ArmyToOptimize.TownHallLevel >= 12)
                            CurrentStep = 4;
                        else
                            CurrentStep = 3;
                    }
                    
                });

            SelectTownHallCommand = new RelayCommand<int>(th =>
            {
                ArmyToOptimize.TownHallLevel = th;

                OnPropertyChanged(nameof(TownHallLevel));

                MaxHousing = GetHousingForTownHall(th);
                MaxSpellHousing = GetSpellCapacityForTownHall(th);
                MaxSiegeMachineHousing = GetSpellsSiegeMachineCapacityForTownHall(th);

                CurrentStep = 2;
            });

            ToggleTroopCommand = new RelayCommand<SelectableTroop>(troop =>
                {
                    troop.IsSelected = !troop.IsSelected;

                    if (!troop.IsSelected)
                        troop.Quantity = 0;

                    OnPropertyChanged(nameof(CurrentHousing));
                });
           
            
                ToggleSiegeMachinesCommand = new RelayCommand<SelectableSiegeMachine>(machine =>
                {
                    machine.IsSelected = !machine.IsSelected;
                    if (!machine.IsSelected)
                        machine.Quantity = 0;
                    UpdateSiegeMachineHousing();
                
                    OnPropertyChanged(nameof(HasSelectedSiegeMachine));
                });
            

                ToggleSpellCommand = new RelayCommand<SelectableSpell>(spell =>
                {
                    spell.IsSelected = !spell.IsSelected;

                    if (!spell.IsSelected)
                        spell.Quantity = 0;

                    UpdateSpellHousing();
                });




            NextCommand = new RelayCommand(_ =>
            {
                if (CurrentStep == 2)
                {
                    CurrentStep = 3;
                }

                else if (CurrentStep == 3)
                {
                    if (ArmyToOptimize.TownHallLevel >= 12)
                        CurrentStep = 4;
                    else
                    {
                        BuildArmyModel();
                        CurrentStep = 4;
                    }
                }

                else if (CurrentStep == 4)
                {
                    BuildArmyModel();   // ALWAYS build before summary
                    CurrentStep = ArmyToOptimize.TownHallLevel >= 12 ? 5 : 4;
                }
            });


            //Go to save army page with the current army configuration
            SaveArmyCommand = new RelayCommand(_ =>
            {
                BuildArmyModel();
                //we send the optimized result army and "this" to save the state of the form if the user wants to come back to the form from the save army page
                _navigation.CurrentView = new SaveArmyVM(_navigation, OptimizedResult, this);
            });
        }
        //--------------------------------------------------------------------------

            private void InitializeCollections()
            {

                Heroes = new ObservableCollection<SelectableHero>
                {
                    new SelectableHero { Name = "Barbarian King" },
                    new SelectableHero { Name = "Archer Queen" },
                    new SelectableHero { Name = "Minion Prince" },
                    new SelectableHero { Name = "Grand Warden" },
                    new SelectableHero { Name = "Royal Champion" }
                };
                ElixirTroops = new ObservableCollection<SelectableTroop>
                {
                    new SelectableTroop { Name="Barbarian", HousingSpace=1 },
                    new SelectableTroop { Name="Archer", HousingSpace=1 },
                    new SelectableTroop { Name="Giant", HousingSpace=5 },
                    new SelectableTroop { Name="Goblin", HousingSpace=1 },
                    new SelectableTroop { Name="Wall Breaker", HousingSpace=2 },
                    new SelectableTroop { Name="Balloon", HousingSpace=5 },
                    new SelectableTroop { Name="Wizard", HousingSpace=4 },
                    new SelectableTroop { Name="Healer", HousingSpace=14 },
                    new SelectableTroop { Name="Dragon", HousingSpace=20 },
                    new SelectableTroop { Name="P.E.K.K.A", HousingSpace=25 },
                    new SelectableTroop { Name="Baby Dragon", HousingSpace=10 },
                    new SelectableTroop { Name="Miner", HousingSpace=6 },
                    new SelectableTroop { Name="Electro Dragon", HousingSpace=30 },
                    new SelectableTroop { Name="Yeti", HousingSpace=18 },
                    new SelectableTroop { Name="Dragon Rider", HousingSpace=25 },
                    new SelectableTroop { Name="Electro Titan", HousingSpace=32 },
                    new SelectableTroop { Name="Root Rider", HousingSpace=20 },
                    new SelectableTroop { Name="Thrower", HousingSpace=16 },
                    new SelectableTroop { Name="Meteor Golem", HousingSpace=40 }
                };

                DarkTroops = new ObservableCollection<SelectableTroop>
                {
                    new SelectableTroop { Name="Minion", HousingSpace=2 },
                    new SelectableTroop { Name="Hog Rider", HousingSpace=5 },
                    new SelectableTroop { Name="Valkyrie", HousingSpace=8 },
                    new SelectableTroop { Name="Golem", HousingSpace=30 },
                    new SelectableTroop { Name="Witch", HousingSpace=12 },
                    new SelectableTroop { Name="Lava Hound", HousingSpace=30 },
                    new SelectableTroop { Name="Bowler", HousingSpace=6 },
                    new SelectableTroop { Name="Ice Golem", HousingSpace=15 },
                    new SelectableTroop { Name="Headhunter", HousingSpace=6 },
                    new SelectableTroop { Name="Apprentice Warden", HousingSpace=20 },
                    new SelectableTroop { Name="Druid", HousingSpace=16 },
                    new SelectableTroop { Name="Furnace", HousingSpace=18 }
                };

                SuperTroops = new ObservableCollection<SelectableTroop>
                {
                    new SelectableTroop { Name="Super Barbarian", HousingSpace=5 },
                    new SelectableTroop { Name="Super Archer", HousingSpace=12 },
                    new SelectableTroop { Name="Super Giant", HousingSpace=10 },
                    new SelectableTroop { Name="Sneaky Goblin", HousingSpace=3 },
                    new SelectableTroop { Name="Super Wall Breaker", HousingSpace=8 },
                    new SelectableTroop { Name="Rocket Balloon", HousingSpace=8 },
                    new SelectableTroop { Name="Super Wizard", HousingSpace=10 },
                    new SelectableTroop { Name="Super Dragon", HousingSpace=40 },
                    new SelectableTroop { Name="Inferno Dragon", HousingSpace=15 },
                    new SelectableTroop { Name="Super Minion", HousingSpace=12 },
                    new SelectableTroop { Name="Super Valkyrie", HousingSpace=20 },
                    new SelectableTroop { Name="Super Witch", HousingSpace=40 },
                    new SelectableTroop { Name="Ice Hound", HousingSpace=40 },
                    new SelectableTroop { Name="Super Bowler", HousingSpace=30 },
                    new SelectableTroop { Name="Super Hog Rider", HousingSpace=12 }
                };


                ElixirSpells = new ObservableCollection<SelectableSpell>
                {
                    new SelectableSpell { Name="Lightning Spell", SpellingHousing = 2 },
                    new SelectableSpell { Name="Healing Spell", SpellingHousing = 2 },
                    new SelectableSpell { Name="Rage Spell", SpellingHousing = 2 },
                    new SelectableSpell { Name="Jump Spell", SpellingHousing = 2 },
                    new SelectableSpell { Name="Freeze Spell", SpellingHousing = 2 },
                    new SelectableSpell { Name="Clone Spell", SpellingHousing = 2 },
                    new SelectableSpell { Name="Invisibility Spell", SpellingHousing = 2 },
                    new SelectableSpell { Name="Recall Spell", SpellingHousing = 2 },
                    new SelectableSpell { Name="Overgrowth Spell", SpellingHousing = 2 },
                };
            
                DarkSpells = new ObservableCollection<SelectableSpell>
                {
                    new SelectableSpell { Name="Poison Spell", SpellingHousing = 1 },
                    new SelectableSpell { Name="Earthquake Spell", SpellingHousing = 2 },
                    new SelectableSpell { Name="Haste Spell", SpellingHousing = 1 },
                    new SelectableSpell { Name="Skeleton Spell", SpellingHousing = 1 },
                    new SelectableSpell { Name="Bat Spell", SpellingHousing = 1 }
                };

                SiegeMachines= new ObservableCollection<SelectableSiegeMachine>
                {
                    new SelectableSiegeMachine { Name="Wall Wrecker", MachineHousing=1 },
                    new SelectableSiegeMachine { Name="Battle Blimp", MachineHousing=1 },
                    new SelectableSiegeMachine { Name="Stone Slammer", MachineHousing=1 },
                    new SelectableSiegeMachine { Name="Siege Barracks", MachineHousing=1 },
                    new SelectableSiegeMachine { Name="Log Launcher", MachineHousing=1 },
                    new SelectableSiegeMachine { Name="Flame Flinger", MachineHousing=1 },
                    new SelectableSiegeMachine { Name="Battle Drill", MachineHousing=1 }
                };

                foreach (var hero in Heroes)
                {
                    hero.PropertyChanged += (s, e) =>
                    {
                        if (e.PropertyName == nameof(SelectableHero.IsSelected))
                            OnPropertyChanged(nameof(HasSelectedHero));
                    };
                }

                var allSpells = ElixirSpells.Concat(DarkSpells);


                foreach (var spell in allSpells)
                {
                    spell.PropertyChanged += (s, e) =>
                    {
                        if (e.PropertyName == nameof(SelectableSpell.Quantity))
                            OnPropertyChanged(nameof(HasSelectedSpell));
                    };
                }

                foreach (var machine in SiegeMachines)
                {
                    machine.PropertyChanged += (s, e) =>
                    {
                        if (e.PropertyName == nameof(SelectableSiegeMachine.Quantity))
                            OnPropertyChanged(nameof(HasSelectedSiegeMachine));
                    };
                }


            }
            private void InitializeTroopCommands()
            {
                void SetupTroop(SelectableTroop troop)
                {
                    troop.IncreaseCommand = new RelayCommand(_ =>
                    {
                        if (CurrentHousing + troop.HousingSpace <= MaxHousing)
                        {
                            troop.Quantity++;
                            UpdateHousing();
                        }
                    });

                    troop.DecreaseCommand = new RelayCommand(_ =>
                    {
                        if (troop.Quantity > 0)
                        {
                            troop.Quantity--;
                            UpdateHousing();
                        }
                    });

                    troop.PropertyChanged += (s, e) =>
                    {
                        if (e.PropertyName == nameof(SelectableTroop.Quantity))
                            UpdateHousing();
                    };
                }
                void SetupSpell(SelectableSpell spell)
                {
                    spell.IncreaseCommand = new RelayCommand(_ =>
                    {
                        if (CurrentSpellHousing + spell.SpellingHousing <= MaxSpellHousing)
                        {
                            spell.Quantity++;
                            UpdateSpellHousing();
                        }
                    });

                    spell.DecreaseCommand = new RelayCommand(_ =>
                    {
                        if (spell.Quantity > 0)
                        {
                            spell.Quantity--;
                            UpdateSpellHousing();
                        }
                    });
                }

                void SetupSiegeMachine(SelectableSiegeMachine machine)
                {
                    machine.IncreaseCommand = new RelayCommand(_ =>
                    {
                        if (CurrentSiegeMachineHousing + machine.MachineHousing <= MaxSiegeMachineHousing)
                        {
                            machine.Quantity++;
                            UpdateSiegeMachineHousing();
                        }
                    });
                    machine.DecreaseCommand = new RelayCommand(_ =>
                    {
                        if (machine.Quantity > 0)
                        {
                            machine.Quantity--;
                            UpdateSiegeMachineHousing();
                        }
                    });
                }

                foreach (var troop in ElixirTroops)
                    SetupTroop(troop);

                foreach (var troop in DarkTroops)
                    SetupTroop(troop);

                foreach (var troop in SuperTroops)
                    SetupTroop(troop);

                foreach (var spell in ElixirSpells)
                    SetupSpell(spell);

                foreach (var darkspell in DarkSpells)
                    SetupSpell(darkspell);

                foreach (var machine in SiegeMachines)
                    SetupSiegeMachine(machine);
            }

                // spinner
                private bool _isLoading;
                public bool IsLoading
                {
                    get => _isLoading;
                    set
                    {
                        _isLoading = value;
                        OnPropertyChanged();
            }
        }

        

            //AI Return army optmized section 

            public ObservableCollection<ArmyUnit> OptimizedTroops { get; set; } = new();
            public ObservableCollection<ArmyUnit> OptimizedSpells { get; set; } = new();
            public ObservableCollection<string> OptimizedHeroes { get; set; } = new();
            public ObservableCollection<HeroLoadout> OptimizedHeroLoadouts { get; set; } = new();


            public string OptimizedSiegeMachine { get; set; }

            public string AiAdvice { get; set; }

        //task to optimize the army
        private async Task Optimize()
        {
            try
            {
                IsLoading = true;

                BuildArmyModel();

                var request = new
                {
                    townHall = ArmyToOptimize.TownHallLevel,
                    heroes = ArmyToOptimize.SelectedHeroes,
                    troops = ArmyToOptimize.SelectedTroops.Select(t => new
                    {
                        name = t.Name,
                        quantity = t.Quantity
                    }),
                    spells = ArmyToOptimize.SelectedSpells.Select(s => new
                    {
                        name = s.Name,
                        quantity = s.Quantity
                    }),
                    siegeMachine = ArmyToOptimize.SelectedSiegeMachines
                        .FirstOrDefault()?.Name
                };

                var result = await _optimizer.OptimizeArmy(request);

                if (result == null)
                {
                    MessageBox.Show("Optimization failed");
                    return;
                }

                OptimizedTroops = new ObservableCollection<ArmyUnit>(result.troops);
                OptimizedSpells = new ObservableCollection<ArmyUnit>(result.spells);
                OptimizedHeroes = new ObservableCollection<string>(result.heroes);
                OptimizedHeroLoadouts = new ObservableCollection<HeroLoadout>(result.heroLoadouts);
                OptimizedSiegeMachine = result.siegeMachine;
                AiAdvice = result.aiNotes;
                
                //heere saves all the optimized result in one to pass it to the save army page
                OptimizedResult = result;

                OnPropertyChanged(nameof(OptimizedTroops));
                OnPropertyChanged(nameof(OptimizedSpells));
                OnPropertyChanged(nameof(OptimizedHeroes));
                OnPropertyChanged(nameof(OptimizedHeroLoadouts));
                OnPropertyChanged(nameof(OptimizedSiegeMachine));
                OnPropertyChanged(nameof(AiAdvice));
                OnPropertyChanged(nameof(OptimizedResult));


                CurrentStep = ArmyToOptimize.TownHallLevel >= 12 ? 6 : 5;
            }
            finally
            {
                IsLoading = false;
            }
        }

       
    }
}