using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Catacomb
{
    public class Game1 : Game
    {
        Texture2D titleScreenTexture;

        private RenderTarget2D renderTarget;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private const int BaseWidth = 512;
        private const int BaseHeight = BaseWidth * 3 / 4;
        private const int RenderScale = 2;
        
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = BaseWidth * RenderScale;
            _graphics.PreferredBackBufferHeight = BaseHeight * RenderScale;
            _graphics.ApplyChanges();

            renderTarget = new RenderTarget2D(GraphicsDevice, BaseWidth, BaseHeight);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            titleScreenTexture = Content.Load<Texture2D>("art/screen/TITLESCREEN");

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
            _spriteBatch.Draw(titleScreenTexture, Vector2.Zero, Color.White);
            _spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            _spriteBatch.Draw(renderTarget, new Rectangle(0, 0, BaseWidth * 2, BaseHeight * 2), Color.White);
            _spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
