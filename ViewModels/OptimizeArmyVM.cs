    using System.Collections.ObjectModel;
    using System.Linq;
using System.Windows;
using ArmyOptimizer.Helpers;
using ArmyOptimizer.Models;
using ArmyOptimizer.Services;
using ArmyOptimizer.Utilities;

    namespace ArmyOptimizer.ViewModels
{
    public class OptimizeArmyVM : ViewModelBase
    {
        private readonly NavigationVM _navigation;


        //Back home and go to save army commands
        public Utilities.RelayCommand BackCommand { get; }
        public Utilities.RelayCommand SaveArmyCommand { get; }
        //------------------------------------------

        //Toggle selection commands for troops,spells and siege machines
        public Utilities.RelayCommand<SelectableTroop> ToggleTroopCommand { get; }
        public Utilities.RelayCommand<SelectableSpell> ToggleSpellCommand { get; }
        public Utilities.RelayCommand<SelectableSiegeMachine> ToggleSiegeMachinesCommand { get; }
        public Utilities.RelayCommand OptimizeCommand { get; }
        //-----------------------------------------------------------------


        //Optimizer army result , this variable will contain the optimized army to pass it to the save armyVM in case the user wants to save it 
        public AiArmyResponse OptimizedResult { get; set; }
        //-------------------------



        //navigation inside the optimizer stepup
        public Utilities.RelayCommand NextCommand { get; }
        public Utilities.RelayCommand PreviousStepCommand { get; }
        //------------------------------------------

        //housing properties for Troops, Spells and Siege Machines
        public bool IsArmyFull => CurrentHousing >= MaxHousing;
        public bool IsSpellFull => CurrentSpellHousing >= MaxSpellHousing;
        public bool IsSiegeFull => CurrentSiegeMachineHousing >= MaxSiegeMachineHousing;
        //------------------------------------------------------------------------------


        //TownHall selection command and properties
        public RelayCommand<int> SelectTownHallCommand { get; }
        public bool HasSelectedHero => Heroes?.Any(h => h.IsSelected) == true;
        public bool HasSelectedSpell => ElixirSpells.Any(s => s.Quantity > 0) || DarkSpells.Any(s => s.Quantity > 0);
        public bool HasSelectedSiegeMachine => SiegeMachines.Any(s => s.Quantity > 0);
        public int TownHallLevel => ArmyToOptimize.TownHallLevel;
        public IEnumerable<SelectableHero> AvailableHeroes =>Heroes.Where(h => h.RequiredTownHall <= TownHallLevel);

        public IEnumerable<SelectableTroop> AvailableElixirTroops =>ElixirTroops.Where(t => t.RequiredTownHall <= TownHallLevel);
        public IEnumerable<SelectableTroop> AvailableDarkTroops =>DarkTroops.Where(t => t.RequiredTownHall <= TownHallLevel);

        public IEnumerable<SelectableTroop> AvailableSuperTroops =>SuperTroops.Where(t => t.RequiredTownHall <= TownHallLevel);

        public IEnumerable<SelectableSpell> AvailableElixirSpells =>ElixirSpells.Where(s => s.RequiredTownHall <= TownHallLevel);

        public IEnumerable<SelectableSpell> AvailableDarkSpells =>DarkSpells.Where(s => s.RequiredTownHall <= TownHallLevel);

        public IEnumerable<SelectableSiegeMachine> AvailableSiegeMachines =>SiegeMachines.Where(s => s.RequiredTownHall <= TownHallLevel);


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
        public bool IsSpellStep => (ArmyToOptimize.TownHallLevel >= 12 && CurrentStep == 4) || (ArmyToOptimize.TownHallLevel < 12 && CurrentStep == 3);
        public bool IsSummaryStep => (ArmyToOptimize.TownHallLevel >= 12 && CurrentStep == 5) || (ArmyToOptimize.TownHallLevel < 12 && CurrentStep == 4);
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
            CurrentSiegeMachineHousing = (SiegeMachines?.Sum(s => s.Quantity * s.MachineHousing) ?? 0);

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


        //this will get the howsing by townhall selected by the user 

        private int GetHousingForTownHall(int th)
        {
            return th switch
            {
                7 => 200,
                8 => 200,
                9 => 220,
                10 => 240,
                11 => 260,
                12 => 280,
                13 => 300,
                14 => 300,
                15 => 320,
                16 => 320,
                17 => 340,
                18 => 340,
                _ => 200
            };
        }

        //this will get the spell capacity by townhall selected by the user
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

        //this will get the siege machine capacity by townhall selected by the user from townhall 12 and above the capacity is 3 siege machines, below that there is no siege machine available
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

        //declaration of the townhall colection :
        public ObservableCollection<TownHallOption> TownHalls { get; set; }

        //summary properties 
        //heroes
        public IEnumerable<SelectableHero> SelectedHeroes => Heroes.Where(h => h.IsSelected);

        //Troops,DarkTroops,SuperTroops
        public IEnumerable<SelectableTroop> SelectedElixirTroops => ElixirTroops.Where(t => t.Quantity > 0);

        public IEnumerable<SelectableTroop> SelectedDarkTroops => DarkTroops.Where(t => t.Quantity > 0);

        public IEnumerable<SelectableTroop> SelectedSuperTroops => SuperTroops.Where(t => t.Quantity > 0);

        //SiegeMachines
        public IEnumerable<SelectableSiegeMachine> SelectedSiegeMachines => SiegeMachines.Where(s => s.Quantity > 0);

        //spells,DarkSpells

        public IEnumerable<SelectableSpell> SelectedElixirSpells => ElixirSpells.Where(s => s.Quantity > 0);

        public IEnumerable<SelectableSpell> SelectedDarkSpells => DarkSpells.Where(s => s.Quantity > 0);

        private readonly AiOptimizerService _optimizer;

        public async Task InitializeAsync()
        {
            IsLoading = true;

            // 1. Inicializar datos (rápido, no hace falta Task.Run)
            await InitializeCollectionsAsync();

            // 2. Inicializar comandos (depende de datos)
            InitializeTroopCommands();

            // 3. Cargar imágenes (esto sí es lo pesado)
            await LoadImagesAsync();

            IsLoading = false;
        }



        //constructor  
        public OptimizeArmyVM(NavigationVM navigation)
        {
            _navigation = navigation;

            _optimizer = new AiOptimizerService(HttpService.Client);

            // Core model
            ArmyToOptimize = new ArmyToOptimize();

            Heroes = new ObservableCollection<SelectableHero>();
            ElixirTroops = new ObservableCollection<SelectableTroop>();
            DarkTroops = new ObservableCollection<SelectableTroop>();
            SuperTroops = new ObservableCollection<SelectableTroop>();
            ElixirSpells = new ObservableCollection<SelectableSpell>();
            DarkSpells = new ObservableCollection<SelectableSpell>();
            SiegeMachines = new ObservableCollection<SelectableSiegeMachine>();
            TownHalls = new ObservableCollection<TownHallOption>();

            // Commands
            OptimizeCommand = new Utilities.RelayCommand(async _ => await Optimize());

            BackCommand = new Utilities.RelayCommand(_ =>
                _navigation.CurrentView = new HomeVM(_navigation));

            PreviousStepCommand = new Utilities.RelayCommand(_ =>
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
               ;

                OnPropertyChanged(nameof(TownHallLevel));
                OnPropertyChanged(nameof(AvailableHeroes));
                OnPropertyChanged(nameof(AvailableElixirTroops));
                OnPropertyChanged(nameof(AvailableDarkTroops));
                OnPropertyChanged(nameof(AvailableSuperTroops));
                OnPropertyChanged(nameof(AvailableElixirSpells));
                OnPropertyChanged(nameof(AvailableDarkSpells));
                OnPropertyChanged(nameof(AvailableSiegeMachines));

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

            NextCommand = new Utilities.RelayCommand(_ =>
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
                    BuildArmyModel();
                    CurrentStep = ArmyToOptimize.TownHallLevel >= 12 ? 5 : 4;
                }
            });

            SaveArmyCommand = new Utilities.RelayCommand(_ =>
            {
                BuildArmyModel();
                _navigation.CurrentView = new SaveArmyVM(_navigation, OptimizedResult, this);
            });
        }
        //--------------------------------------------------------------------------

        public async Task InitializeCollectionsAsync()
        {
            {

                TownHalls = new ObservableCollection<TownHallOption>
                {
                    new() { Level = 7, ImageUrl = "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774375343/Town_Hall7_ll63f5.png" },
                    new() { Level = 8, ImageUrl = "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774375343/Town_Hall8_iaxn0n.png" },
                    new() { Level = 9, ImageUrl = "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774375343/Town_Hall9_zstrie.png" },
                    new() { Level = 10, ImageUrl = "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774375342/Town_Hall10_eq4hio.png" },
                    new() { Level = 11, ImageUrl = "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774375342/Town_Hall11_vqtc8g.png" },
                    new() { Level = 12, ImageUrl = "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774375341/Town_Hall12-5_fmb4yw.png" },
                    new() { Level = 13, ImageUrl = "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774375341/Town-hall-13-5_syw8cr.png" },
                    new() { Level = 14, ImageUrl = "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774375342/Town_Hall14-5_qpsnsf.png" },
                    new() { Level = 15, ImageUrl = "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774375342/Town_Hall15-5_zbyib8.png" },
                    new() { Level = 16, ImageUrl = "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774375342/Town_Hall16_loputx.png" },
                    new() { Level = 17, ImageUrl = "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774375343/Town_Hall17-5_z0prjh.png" },
                    new() { Level = 18, ImageUrl = "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774375342/Town_Hall18_e9l7v3.png" }
                };

                OnPropertyChanged(nameof(TownHalls));

                Heroes = new ObservableCollection<SelectableHero>
                {
                new SelectableHero
                {
                    Name = "Barbarian King",
                    ImageUrl = "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774375917/Avatar_Hero_Barbarian_King_s87uct.png",
                    RequiredTownHall = 7,

                },
                new SelectableHero
                {
                    Name = "Archer Queen",
                    ImageUrl = "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774375916/Avatar_Hero_Archer_Queen_peli8n.png",
                    RequiredTownHall = 9
                },
                new SelectableHero
                {
                    Name = "Grand Warden",
                    ImageUrl = "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774375916/Avatar_Hero_Grand_Warden_quwu8o.png",
                    RequiredTownHall = 11
                },
                new SelectableHero
                {
                    Name = "Minion Prince",
                    ImageUrl = "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774375916/Avatar_Hero_Minion_Prince_b9nkfj.png",
                    RequiredTownHall = 9
                },
                new SelectableHero
                {
                    Name = "Royal Champion",
                    ImageUrl = "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774375917/Avatar_Hero_Royal_Champion_iu0z2e.png",
                    RequiredTownHall = 13
                }
            };
                ElixirTroops = new ObservableCollection<SelectableTroop>
            {
                new SelectableTroop { Name="Barbarian", HousingSpace=1, ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376169/Avatar_Barbarian_lwtrbq.png",RequiredTownHall = 1 },
                new SelectableTroop { Name="Archer", HousingSpace=1, ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376167/Avatar_Archer_p8fvxs.png", RequiredTownHall=2 },
                new SelectableTroop { Name="Giant", HousingSpace=5, ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376170/Avatar_Giant_ddigmo.png", RequiredTownHall=3  },
                new SelectableTroop { Name="Goblin", HousingSpace=1, ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376168/Avatar_Goblin_iiwpog.png" , RequiredTownHall=3 },
                new SelectableTroop { Name="Wall Breaker", HousingSpace=2, ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376174/Avatar_Wall_Breaker_z7i4jg.png" ,RequiredTownHall=4 },
                new SelectableTroop { Name="Balloon", HousingSpace=5, ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376166/Avatar_Balloon_gxewyx.png",RequiredTownHall = 4  },
                new SelectableTroop { Name="Wizard", HousingSpace=4, ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376175/Avatar_Wizard_pnwv2h.png",RequiredTownHall = 5 },
                new SelectableTroop { Name="Healer", HousingSpace=14, ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376166/Avatar_Healer_ahurvo.png",RequiredTownHall = 6 },
                new SelectableTroop { Name="Dragon", HousingSpace=20, ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376166/Avatar_Dragon_rgvc1t.png",RequiredTownHall = 7 },
                new SelectableTroop { Name="P.E.K.K.A", HousingSpace=25, ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376173/Avatar_P.E.K.K.A_uiwpd6.png",RequiredTownHall = 8 },
                new SelectableTroop { Name="Baby Dragon", HousingSpace=10, ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376171/Avatar_Baby_Dragon_lano31.png",RequiredTownHall = 8 },
                new SelectableTroop { Name="Miner", HousingSpace=6, ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376172/Avatar_Miner_u50jh2.png" ,RequiredTownHall = 10},
                new SelectableTroop { Name="Electro Dragon", HousingSpace=30, ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376168/Avatar_Electro_Dragon_pk9esh.png",RequiredTownHall = 11 },
                new SelectableTroop { Name="Yeti", HousingSpace=18, ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376175/Avatar_Yeti_qmbs18.png",RequiredTownHall = 12 },
                new SelectableTroop { Name="Dragon Rider", HousingSpace=25, ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376170/Avatar_Dragon_Rider_hva940.png",RequiredTownHall = 13 },
                new SelectableTroop { Name="Electro Titan", HousingSpace=32, ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376170/Avatar_Electro_Titan_lgs4ee.png",RequiredTownHall = 14 },
                new SelectableTroop { Name="Root Rider", HousingSpace=20, ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376173/Avatar_Root_Rider_wynbsd.png",RequiredTownHall = 15 },
                new SelectableTroop { Name="Thrower", HousingSpace=16, ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376174/Avatar_Thrower_ughnfy.png",RequiredTownHall = 16 },
                new SelectableTroop { Name="Meteor Golem", HousingSpace=40, ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376172/Avatar_Meteor_Golem_ecr7w8.png",RequiredTownHall = 17 }
            };


                DarkTroops = new ObservableCollection<SelectableTroop>
                    {
                        new SelectableTroop
                        {
                            Name="Minion",
                            HousingSpace=2,
                            ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376407/Avatar_Minion_miyqch.png",
                            RequiredTownHall = 7
                        },

                        new SelectableTroop
                        {
                            Name="Hog Rider",
                            HousingSpace=5,
                            ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376414/Avatar_Hog_Rider_gnx8mx.png",
                            RequiredTownHall = 7
                        },

                        new SelectableTroop
                        {
                            Name="Valkyrie",
                            HousingSpace=8,
                            ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376415/Avatar_Valkyrie_xfxukv.png",
                            RequiredTownHall = 8
                        },

                        new SelectableTroop
                        {
                            Name="Golem",
                            HousingSpace=30,
                            ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376413/Avatar_Golem_vef0wn.png",
                            RequiredTownHall = 8
                        },

                        new SelectableTroop
                        {
                            Name="Witch",
                            HousingSpace=12,
                            ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376411/Avatar_Witch_mtqi6z.png",
                            RequiredTownHall = 9
                        },

                        new SelectableTroop
                        {
                            Name="Lava Hound",
                            HousingSpace=30,
                            ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376410/Avatar_Lava_Hound_bok88r.png",
                            RequiredTownHall = 9
                        },

                        new SelectableTroop
                        {
                            Name="Bowler",
                            HousingSpace=6,
                            ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376407/Avatar_Bowler_inluv7.png",
                            RequiredTownHall = 10
                        },

                        new SelectableTroop
                        {
                            Name="Ice Golem",
                            HousingSpace=15,
                            ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376413/Avatar_Ice_Golem_gdaagq.png",
                            RequiredTownHall = 11
                        },

                        new SelectableTroop
                        {
                            Name="Headhunter",
                            HousingSpace=6,
                            ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376411/Avatar_Headhunter_mlfchb.png",
                            RequiredTownHall = 12
                        },

                        new SelectableTroop
                        {
                            Name="Apprentice Warden",
                            HousingSpace=20,
                            ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376406/Avatar_Apprentice_Warden_skdqyq.png",
                            RequiredTownHall = 13
                        },

                        new SelectableTroop
                        {
                            Name="Druid",
                            HousingSpace=16,
                            ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376416/Avatar_Druid_kxagut.png",
                            RequiredTownHall = 16
                        },

                        new SelectableTroop
                        {
                            Name="Furnace",
                            HousingSpace=18,
                            ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376409/Avatar_Furnace_jvxkus.png",
                            RequiredTownHall = 17
                        }
                    };

                SuperTroops = new ObservableCollection<SelectableTroop>
                    {
                        new SelectableTroop { Name="Super Barbarian", HousingSpace=5, ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376772/Avatar_Super_Barbarian_fphb14.png",RequiredTownHall = 11  },
                        new SelectableTroop { Name="Super Archer", HousingSpace=12, ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376771/Avatar_Super_Archer_zt7amw.png",RequiredTownHall = 11 },
                        new SelectableTroop { Name="Super Giant", HousingSpace=10, ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376777/Avatar_Super_Giant_wetksa.png" ,RequiredTownHall = 11},
                        new SelectableTroop { Name="Sneaky Goblin", HousingSpace=3, ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376768/Avatar_Sneaky_Goblin_g4kro0.png" },
                        new SelectableTroop { Name="Super Wall Breaker", HousingSpace=8, ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376776/Avatar_Super_Wall_Breaker_swt7rd.png",RequiredTownHall = 11 },
                        new SelectableTroop { Name="Rocket Balloon", HousingSpace=8, ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376773/Avatar_Rocket_Balloon_try2b2.png" ,RequiredTownHall = 11},
                        new SelectableTroop { Name="Super Wizard", HousingSpace=10, ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376782/Avatar_Super_Wizard_i32ge7.png" ,RequiredTownHall = 11},
                        new SelectableTroop { Name="Super Dragon", HousingSpace=40, ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376775/Avatar_Super_Dragon_asqh6m.png",RequiredTownHall = 11 },
                        new SelectableTroop { Name="Inferno Dragon", HousingSpace=15, ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376769/Avatar_Inferno_Dragon_frhlmg.png",RequiredTownHall = 11 },

                        new SelectableTroop { Name="Super Miner", HousingSpace=12, ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376770/Avatar_Super_Miner_zvyz0b.png",RequiredTownHall = 11 },
                        new SelectableTroop { Name="Super Yeti", HousingSpace=18, ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376782/Avatar_Super_Yeti_cddyyh.png",RequiredTownHall = 11 },

                        new SelectableTroop { Name="Super Minion", HousingSpace=12, ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376779/Avatar_Super_Minion_x1ipym.png" ,RequiredTownHall = 11},
                        new SelectableTroop { Name="Super Valkyrie", HousingSpace=20, ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376767/Avatar_Super_Valkyrie_ngqc64.png",RequiredTownHall = 11 },
                        new SelectableTroop { Name="Super Witch", HousingSpace=40, ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376780/Avatar_Super_Witch_mpdgfd.png",RequiredTownHall = 11 },
                        new SelectableTroop { Name="Ice Hound", HousingSpace=40, ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376767/Avatar_Ice_Hound_zs2pne.png",RequiredTownHall = 11 },
                        new SelectableTroop { Name="Super Bowler", HousingSpace=30, ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376774/Avatar_Super_Bowler_veuaf3.png",RequiredTownHall = 11 },
                        new SelectableTroop { Name="Super Hog Rider", HousingSpace=12, ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376778/Avatar_Super_Hog_Rider_lxpiic.png",RequiredTownHall = 11 }
                    };



                ElixirSpells = new ObservableCollection<SelectableSpell>
                       {
                new SelectableSpell
                {
                    Name="Lightning Spell",
                    SpellingHousing=2,
                    ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774378218/Lightning_Spell_info_wd7hkz.png",
                    RequiredTownHall = 5
                },

                new SelectableSpell
                {
                    Name="Healing Spell",
                    SpellingHousing=2,
                    ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774378209/Healing_Spell_info_kobwfq.png",
                    RequiredTownHall = 6
                },

                new SelectableSpell
                {
                    Name="Rage Spell",
                    SpellingHousing=2,
                    ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774378209/Rage_Spell_info_cf9p9f.png",
                    RequiredTownHall = 7
                },

                new SelectableSpell
                {
                    Name="Jump Spell",
                    SpellingHousing=2,
                    ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774378217/Jump_Spell_info_lscait.png",
                    RequiredTownHall = 9
                },

                new SelectableSpell
                {
                    Name="Freeze Spell",
                    SpellingHousing=2,
                    ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774378213/Freeze_Spell_info_dmplps.png",
                    RequiredTownHall = 10
                },

                new SelectableSpell
                {
                    Name="Clone Spell",
                    SpellingHousing=2,
                    ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774378215/Clone_Spell_info_n8xesr.png",
                    RequiredTownHall = 11
                },

                new SelectableSpell
                {
                    Name="Invisibility Spell",
                    SpellingHousing=2,
                    ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774378216/Invisibility_Spell_info_fhqu0g.png",
                    RequiredTownHall = 12
                },

                new SelectableSpell
                {
                    Name="Recall Spell",
                    SpellingHousing=2,
                    ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774378219/Recall_Spell_info_wzchax.png",
                    RequiredTownHall = 13
                },

                new SelectableSpell
                {
                    Name="Revive Spell",
                    SpellingHousing=2,
                    ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774378213/Revive_Spell_info_rnvsny.png",
                    RequiredTownHall = 14
                },

                new SelectableSpell
                {
                    Name="Totem Spell",
                    SpellingHousing=2,
                    ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774378214/Totem_Spell_info_akrugc.png",
                    RequiredTownHall = 16
                }
            };

                DarkSpells = new ObservableCollection<SelectableSpell>
            {
                    new SelectableSpell
                    {
                        Name="Poison Spell",
                        SpellingHousing=1,
                        ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774379040/Poison_Spell_info_hkhqfs.png",
                        RequiredTownHall = 8
                    },

                    new SelectableSpell
                    {
                        Name="Earthquake Spell",
                        SpellingHousing=2,
                        ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774379038/Earthquake_Spell_info_b7anqy.png",
                        RequiredTownHall = 8
                    },

                    new SelectableSpell
                    {
                        Name="Haste Spell",
                        SpellingHousing=1,
                        ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774379031/Haste_Spell_info_ceyyeo.png",
                        RequiredTownHall = 9
                    },

                    new SelectableSpell
                    {
                        Name="Skeleton Spell",
                        SpellingHousing=1,
                        ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774379033/Skeleton_Spell_info_nbn76m.png",
                        RequiredTownHall = 9
                    },

                    new SelectableSpell
                    {
                        Name="Bat Spell",
                        SpellingHousing=1,
                        ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774379037/Bat_Spell_info_wa7pvn.png",
                        RequiredTownHall = 10
                    },

                    new SelectableSpell
                    {
                        Name="Overgrowth Spell",
                        SpellingHousing=2,
                        ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774379036/Overgrowth_Spell_info_pe6duh.png",
                        RequiredTownHall = 12
                    },

                    new SelectableSpell
                    {
                        Name="Ice Block Spell",
                        SpellingHousing=1,
                        ImageUrl="https://res.cloudinary.com/dibrwiwx5/image/upload/v1774379034/Ice_Block_Spell_info_dakpyu.png",
                        RequiredTownHall = 14
                    }
            };

                SiegeMachines = new ObservableCollection<SelectableSiegeMachine>
                {
                    new SelectableSiegeMachine
                    {
                        Name = "Wall Wrecker",
                        MachineHousing = 1,
                        ImageUrl = "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774378760/Avatar_Wall_Wrecker_icdrng.png",
                        RequiredTownHall = 12
                    },

                    new SelectableSiegeMachine
                    {
                        Name = "Battle Blimp",
                        MachineHousing = 1,
                        ImageUrl = "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774378758/Avatar_Battle_Blimp_g5wpna.png",
                        RequiredTownHall = 12
                    },

                    new SelectableSiegeMachine
                    {
                        Name = "Stone Slammer",
                        MachineHousing = 1,
                        ImageUrl = "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774378757/Avatar_Stone_Slammer_up7cpv.png",
                        RequiredTownHall = 12
                    },

                    new SelectableSiegeMachine
                    {
                        Name = "Siege Barracks",
                        MachineHousing = 1,
                        ImageUrl = "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774378761/Avatar_Siege_Barracks_xekb2e.png",
                        RequiredTownHall = 13
                    },

                    new SelectableSiegeMachine
                    {
                        Name = "Log Launcher",
                        MachineHousing = 1,
                        ImageUrl = "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774378763/Avatar_Log_Launcher_lmgcyw.png",
                        RequiredTownHall = 13
                    },

                    new SelectableSiegeMachine
                    {
                        Name = "Flame Flinger",
                        MachineHousing = 1,
                        ImageUrl = "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774378756/Avatar_Flame_Flinger_uok6nn.png",
                        RequiredTownHall = 14
                    },

                    new SelectableSiegeMachine
                    {
                        Name = "Battle Drill",
                        MachineHousing = 1,
                        ImageUrl = "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774378755/Avatar_Battle_Drill_c3aa3g.png",
                        RequiredTownHall = 15
                    }
                };

                OnPropertyChanged(nameof(TownHalls));
                OnPropertyChanged(nameof(Heroes));
                OnPropertyChanged(nameof(ElixirTroops));
                OnPropertyChanged(nameof(DarkTroops));
                OnPropertyChanged(nameof(SuperTroops));
                OnPropertyChanged(nameof(ElixirSpells));
                OnPropertyChanged(nameof(DarkSpells));
                OnPropertyChanged(nameof(SiegeMachines));

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
        }
        private void InitializeTroopCommands()
        {
            void SetupTroop(SelectableTroop troop)
            {
                troop.IncreaseCommand = new Utilities.RelayCommand(_ =>
                {
                    if (CurrentHousing + troop.HousingSpace <= MaxHousing)
                    {
                        troop.Quantity++;
                        UpdateHousing();
                    }
                });

                troop.DecreaseCommand = new Utilities.RelayCommand(_ =>
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
                spell.IncreaseCommand = new Utilities.RelayCommand(_ =>
                {
                    if (CurrentSpellHousing + spell.SpellingHousing <= MaxSpellHousing)
                    {
                        spell.Quantity++;
                        UpdateSpellHousing();
                    }
                });

                spell.DecreaseCommand = new Utilities.RelayCommand(_ =>
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
                machine.IncreaseCommand = new Utilities.RelayCommand(_ =>
                {
                    if (CurrentSiegeMachineHousing + machine.MachineHousing <= MaxSiegeMachineHousing)
                    {
                        machine.Quantity++;
                        UpdateSiegeMachineHousing();
                    }
                });
                machine.DecreaseCommand = new Utilities.RelayCommand(_ =>
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

        //spiner optimize

        private bool _isHardLoading;

        public bool IsHardLoading
        {
            get => _isHardLoading;
            set
            {
                _isHardLoading = value;
                OnPropertyChanged();
            }
        }


        //AI Return army optmized section 

        public ObservableCollection<ArmyUnit> OptimizedTroops { get; set; } = new();
        public ObservableCollection<ArmyUnit> OptimizedSpells { get; set; } = new();
        public ObservableCollection<string> OptimizedHeroes { get; set; } = new();
        public ObservableCollection<HeroLoadout> OptimizedHeroLoadouts { get; set; } = new();
        public ObservableCollection<ArmyUnit> OptimizedSiegeMachines { get; set; } = new();
        public string AiAdvice { get; set; }

        //task to optimize the army
        private async Task Optimize()
        {
            try
            {
                IsHardLoading = true;

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
                    siegeMachines = ArmyToOptimize.SelectedSiegeMachines.Select(s => new
                    {
                        name = s.Name,
                        quantity = s.Quantity
                    }).ToList()
                };

                var result = await _optimizer.OptimizeArmy(request);

                if (result == null)
                {
                    ToastService.Instance.Show("Optimization failed, try again", "error");
                    return;
                }

                OptimizedTroops = new ObservableCollection<ArmyUnit>(result.troops);
                OptimizedSpells = new ObservableCollection<ArmyUnit>(result.spells);
                OptimizedHeroes = new ObservableCollection<string>(result.heroes);
                OptimizedHeroLoadouts = new ObservableCollection<HeroLoadout>(result.heroLoadouts);
                OptimizedSiegeMachines = new ObservableCollection<ArmyUnit>(result.siegeMachines);
                AiAdvice = result.aiNotes;

                await LoadOptimizedImagesAsync();
                //heere saves all the optimized result in one to pass it to the save army page
                OptimizedResult = result;

                OnPropertyChanged(nameof(OptimizedTroops));
                OnPropertyChanged(nameof(OptimizedSpells));
                OnPropertyChanged(nameof(OptimizedHeroes));
                OnPropertyChanged(nameof(OptimizedHeroLoadouts));
                OnPropertyChanged(nameof(OptimizedSiegeMachines));
                OnPropertyChanged(nameof(AiAdvice));
                OnPropertyChanged(nameof(OptimizedResult));


                CurrentStep = ArmyToOptimize.TownHallLevel >= 12 ? 6 : 5;
            }
            finally
            {
                IsHardLoading = false;
            }
        }
        //load images for all the army requirments
        private async Task LoadImagesAsync()
        {
            var allItems =
                TownHalls.Select(t => (obj: (dynamic)t, t.ImageUrl))
                .Concat(Heroes.Select(h => ((dynamic)h, h.ImageUrl)))
                .Concat(ElixirTroops.Select(t => ((dynamic)t, t.ImageUrl)))
                .Concat(DarkTroops.Select(t => ((dynamic)t, t.ImageUrl)))
                .Concat(SuperTroops.Select(t => ((dynamic)t, t.ImageUrl)))
                .Concat(ElixirSpells.Select(s => ((dynamic)s, s.ImageUrl)))
                .Concat(DarkSpells.Select(s => ((dynamic)s, s.ImageUrl)))
                .Concat(SiegeMachines.Select(s => ((dynamic)s, s.ImageUrl)));

            foreach (var (item, url) in allItems)
            {
                var image = await ImageCacheService.LoadAsync(url);

                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    item.Image = image;
                });
            }
        }

        private async Task LoadOptimizedImagesAsync()
        {
            // TROOPS
            foreach (var troop in OptimizedTroops)
            {
                if (GameImageLibrary.TroopImages.TryGetValue(troop.Name, out var url))
                {
                    troop.Image = await ImageCacheService.LoadAsync(url);
                }
            }

            // SPELLS
            foreach (var spell in OptimizedSpells)
            {
                if (GameImageLibrary.SpellImages.TryGetValue(spell.Name, out var url))
                {
                    spell.Image = await ImageCacheService.LoadAsync(url);
                }
            }

            // SIEGE
            foreach (var siege in OptimizedSiegeMachines)
            {
                if (GameImageLibrary.SiegeImages.TryGetValue(siege.Name, out var url))
                {
                    siege.Image = await ImageCacheService.LoadAsync(url);
                }
            }

            // HEROES + PETS
            foreach (var hero in OptimizedHeroLoadouts)
            {
                if (GameImageLibrary.HeroImages.TryGetValue(hero.HeroName, out var url))
                {
                    hero.Image = await ImageCacheService.LoadAsync(url);
                }

                if (!string.IsNullOrEmpty(hero.PetName) &&
                    GameImageLibrary.Pets.TryGetValue(hero.PetName, out var petUrl))
                {
                    hero.PetImage = await ImageCacheService.LoadAsync(petUrl);
                }
            }

            // refrescar UI
            OnPropertyChanged(nameof(OptimizedTroops));
            OnPropertyChanged(nameof(OptimizedSpells));
            OnPropertyChanged(nameof(OptimizedSiegeMachines));
            OnPropertyChanged(nameof(OptimizedHeroLoadouts));
        }
    }
}