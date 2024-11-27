using NEKI.Elements.Alive;
using NEKI.Elements.Objects;
using NEKI.Elements.UsefulStuff;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEKI
{
    public class Map
    {

        public Player Player { get; set; }

        public Dictionary<Position, Enemy> enemies = new Dictionary<Position, Enemy>();

        public Dictionary<Position, Ammo> ammos = new Dictionary<Position, Ammo>();

        public List<Bullet> bullets = new List<Bullet>();
        public Tiles Tiles { get; set; }

        Random rnd = new Random();

        public int Width { get; private set; }
        public int Height { get; private set; }

        public int Health { get; private set; }

        public int Damage {  get; private set; }

        public int EnemyDamage {  get; private set; }

        public int PlayerSteps {  get; private set; }

        public int EnemySteps {  get; private set; }

        public int NumberOfEnemies {  get; private set; }

        public int EnemyHealth {  get; private set; }

        public int PlayerDamage { get; private set; }

        bool gameOver = false;

        private const int enemySpeed = 3;

        private const int bulletSpeed = 10;

        bool once = true;

        private bool playerTurn = true;

        Stopwatch playerTimer;
        Stopwatch bulletTimer;
        Stopwatch enemyTimer;
        public Map(int width, int height, int enemySteps, int playerSteps, int enemyHealth, int numberOfEnemies, int enemyDamage, int playerDamage)
        {
            Tiles tiles = new Tiles(width, height);
            Tiles = tiles;
            Width = width;
            Height = height;
            EnemySteps = enemySteps;
            PlayerSteps = playerSteps;
            EnemyHealth = enemyHealth;
            EnemyDamage = enemyDamage;
            NumberOfEnemies = numberOfEnemies;
            PlayerDamage = playerDamage;
            playerTimer = Stopwatch.StartNew();
            bulletTimer = Stopwatch.StartNew();
            enemyTimer = Stopwatch.StartNew();

            tiles.InitializeMap();
            AddEnemies(Health, Damage, numberOfEnemies);
            AddPlayer();
        }

        public void MovePlayer(Direction direction)
        {
            if (playerTimer.ElapsedMilliseconds * Player.Speed > 1000)
            {
                if (Player.Steps > 0)
                {
                    Player.Move(Tiles, direction, ammos);
                    playerTimer = Stopwatch.StartNew();
                    if (Player.Steps <= 0)
                    {
                        playerTurn = false;
                    }
                    if (Player.Health <= 0)
                    {
                        gameOver = true;
                    }
                }
            }

        }

        public void MoveEnemies()
        {
            if (enemyTimer.ElapsedMilliseconds * enemySpeed > 1000 && bullets.Count == 0)
            {
                if (playerTurn == false)
                {

                    foreach (Enemy enemy in enemies.Values)
                    {
                        Bullet? bullet = enemy.Move(Tiles, Player, ammos);
                        if (bullet != null)
                        {
                            bullets.Add(bullet);
                        }
                        enemy.Moved();
                    }
                    enemyTimer = Stopwatch.StartNew();
                    if (enemies.Values.Count > 0 && enemies.Values.First().Steps <= 0)
                    {
                        playerTurn = true;
                    }
                    once = true;

                }
                else if (once)
                {
                    Player.StartMoving(PlayerSteps);
                    foreach (Enemy enemy in enemies.Values)
                    {
                        enemy.StartMoving(EnemySteps);
                    }
                    once = false;
                }
            }
        }

        public void PlayerShoot()
        {
            if (playerTurn)
            {
                Bullet? bullet = Player.Shoot();
                if (bullet != null)
                {
                    if (Tiles.GetObjectFromPosition(bullet.Position) == null)
                    {
                        Tiles.AddNewObjectToPosition(bullet);
                        bullets.Add(bullet);

                    }
                    else if (Tiles.GetObjectFromPosition(bullet.Position).Type == ObjectType.Enemy)
                    {
                        Enemy enemy = enemies.Values.FirstOrDefault(e => e.Position.X == bullet.Position.X && e.Position.Y == bullet.Position.Y);
                        if (enemy.Shot(bullet.Damage))
                        {
                            FindAndDelete(enemy.Position);
                            Tiles.RemoveObjectFromPosition(enemy.Position);
                            Ammo ammo = new Ammo(rnd.Next(1, 10), rnd.Next(1, 5), enemy.Position);
                            Tiles.AddNewObjectToPosition(ammo);
                            ammos.Add(enemy.Position, ammo);
                        }
                    }
                }
            }
        }

        public void UpdateMap()
        {

            if (bulletTimer.ElapsedMilliseconds * bulletSpeed > 1000)
            {
                for (int i = 0; i < bullets.Count(); i++)
                {
                    Bullet bullet = bullets[i];
                    Position oldPosition = new Position(bullet.Position);
                    Position newPosition = bullet.GetNewPosition(bullet.Direction);

                    if (Tiles.GetObjectFromPosition(newPosition) == null)
                    {
                        Tiles.RemoveObjectFromPosition(oldPosition);
                        bullet.UpdatePosition(newPosition);
                        Tiles.AddNewObjectToPosition(bullet);


                    }
                    else if (Tiles.GetObjectFromPosition(newPosition).Type == ObjectType.Enemy && bullet.Players == true)
                    {
                        Enemy enemy = enemies.Values.FirstOrDefault(e => e.Position.X == newPosition.X && e.Position.Y == newPosition.Y);
         
                        if (enemy.Shot(bullet.Damage))
                        {
                            Player.AddScore();
                            FindAndDelete(enemy.Position);
                            Tiles.RemoveObjectFromPosition(enemy.Position);
                            Ammo ammo = new Ammo(rnd.Next(1, 10), rnd.Next(1, 5), enemy.Position);
                            Tiles.AddNewObjectToPosition(ammo);
                            ammos.Add(enemy.Position, ammo);
                        }
                        bullets.RemoveAt(i);
                        i--;
                        Tiles.RemoveObjectFromPosition(oldPosition);
                    }
                    else if (Tiles.GetObjectFromPosition(newPosition).Type == ObjectType.Player && bullet.Players == false)
                    {

                        if (Player.Shot(bullet.Damage))
                        {
                            gameOver = true;
                            Tiles.RemoveObjectFromPosition(newPosition);
                        }
                        bullets.RemoveAt(i);
                        i--;
                        Tiles.RemoveObjectFromPosition(oldPosition);
                    }
                    else
                    {
                        bullets.RemoveAt(i);
                        i--;
                        Tiles.RemoveObjectFromPosition(bullet.Position);
                    }
                }
                bulletTimer = Stopwatch.StartNew();
            }

        }

        private void FindAndDelete(Position pos)
        {
            foreach(Position position in enemies.Keys)
            {
                if (position.X == pos.X && position.Y == pos.Y)
                {
                    enemies.Remove(position);
                }
            }
        }

        public bool GameOver()
        {
            if (Player.Health <= 0)
            {
                return true;
            }
            else
            {
                return gameOver;
            }
        }

        private void AddEnemies(int health, int damage, int numberOfEnemies)
        {
            Random rand = new Random();
            for (int i = 0; i < numberOfEnemies; i++)
            {
                while (true)
                {
                    int x = rand.Next(1, Width - 1);
                    int y = rand.Next(1, Height - 1);

                    if (Tiles.GetObjectFromPosition(new Position(x, y)) == null)
                    {
                        Position position = new Position(x, y);
                        Enemy enemy = new Enemy(position, EnemyHealth, EnemyDamage);
                        enemies[position] = enemy;
                        Tiles.AddNewObjectToPosition(enemy);
                        break;
                    }
                }
            }
        }

        private void AddPlayer()
        {
            Random rand = new Random();
            while (true)
            {
                int x = rand.Next(1, Width - 1);
                int y = rand.Next(1, Height - 1);

                if (Tiles.GetObjectFromPosition(new Position(x, y)) == null)
                {
                    Player player = new Player(new Position(x, y), PlayerSteps, PlayerDamage);
                    Player = player;
                    Tiles.AddNewObjectToPosition(player);
                    break;
                }
            }
        }

    }
}
