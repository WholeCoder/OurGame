using System;

using System.Collections.Generic;

using System.Linq;

using System.Text;

using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Audio;

using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.GamerServices;

using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework.Input;

using Microsoft.Xna.Framework.Media;



namespace BreakOut

{

	class Ball

	{

		// Describes the motion of the ball

		Vector2 motion;

		// Position of the ball

		Vector2 position;

		// Describes the speed of the ball

		float ballSpeed = 4;



		Texture2D texture;

		Rectangle screenBounds;



		public Ball(Texture2D texture, Rectangle screenBounds)

		{

			this.texture = texture;

			this.screenBounds = screenBounds;

		}



		// To update the position of the ball

		public void Update()

		{

			position += motion * ballSpeed;

			CheckWallCollision();

		}

		// Make sure the ball doesn't go off the right, left, top or bottom of the screen

		private void CheckWallCollision()

		{

			if (position.X < 0)

			{

				position.X = 0;

				motion *= -1;

			}



			if (position.X + texture.Width > screenBounds.Width)

			{

				position.X = screenBounds.Width - texture.Width;

				motion.X *= -1;

			}



			if (position.Y < 0)

			{

				position.Y = 0;

				motion.Y *= -1;

			}

		}

		// Describes the paddles location

		public void SetInStartPosition(Rectangle paddleLocation)

		{

			motion = new Vector2(1, -1);

			position.Y = paddleLocation.Y - texture.Height;

			position.X = paddleLocation.X + (paddleLocation.Width - texture.Width) / 2;

		}

		// Checks to see if the ball has fallen off the bottom of the screen

		public bool offBottom()

		{

			if (position.Y > screenBounds.Height)

			{

				return true;

			}



			return false;



		}



		// Checks to see if the ball collides with the paddle

		public void PaddleCollision(Rectangle paddleLocation)

		{

			Rectangle ballLocation = new Rectangle((int) position.X, (int) position.Y, texture.Width, texture.Height);



			if (paddleLocation.Intersects(ballLocation))

			{

				position.Y = paddleLocation.Y - texture.Height;

				motion.Y *= -1;

			}

		}

		// Draws the ball

		public void Draw(SpriteBatch spriteBatch)

		{

			spriteBatch.Draw(texture, position, Color.White);

		}

	}

}









using System;

using System.Collections.Generic;

using System.Linq;

using System.Text;

using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Audio;

using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.GamerServices;

using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework.Input;

using Microsoft.Xna.Framework.Media;



namespace BreakOut

{

	class Brick

	{

		Texture2D texture;

		Rectangle location;

		Color tint;

		bool alive;



		public Rectangle Location

		{

			get {
				return location;
			}

		}



		public Brick(Texture2D texture, Rectangle location, Color tint)

		{

			this.texture = texture;

			this.location = location;

			this.tint = tint;

			this.alive = true;

		}



		public void CheckCollision(Ball ball)

		{}



		public void Draw(SpriteBatch spriteBatch)

		{

			if (alive)

			{

				spriteBatch.Draw(texture, location, tint);

			}

		}

	}

}