using blackbox.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace blackbox.Graphics;

public class SafeSpriteBatch
{
    private SpriteBatch _spriteBatch;
    private bool _begun;

    public SafeSpriteBatch(SpriteBatch spriteBatch)
    {
        _spriteBatch = spriteBatch;
    }

    public void Begin(SpriteSortMode sort, BlendState blendState, SamplerState sampleState, Effect effect)
    {
        _spriteBatch.Begin(sort, blendState, sampleState, null, null, effect);
        _begun = true;
    }

    public void Begin(SamplerState samplerState)
    {
        _spriteBatch.Begin(samplerState: samplerState);
        _begun = true;
    }

    public void Begin(Effect effect)
    {
        _spriteBatch.Begin(effect: effect, samplerState: SamplerState.PointClamp, transformMatrix: Camera2D.GetViewMatrix());
        _begun = true;
    }

    public void Begin()
    {
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: Camera2D.GetViewMatrix());
        _begun = true;
    }

    public void End()
    {
        _spriteBatch.End();
        _begun = false;
    }

    public void Draw(Texture2D texture, Rectangle destination, Rectangle source, Color color, SpriteEffects effects)
    {
        _spriteBatch.Draw(texture, destination, source, color, 0f, Vector2.Zero, effects, 0f);
    }

    public void Draw(Texture2D texture, Rectangle dest, Rectangle source, Color color)
    {
        _spriteBatch.Draw(texture, dest, source, color);
    }

    public void Draw(RenderTarget2D sceneTarget, Rectangle boxToDraw, Color white)
    {
        _spriteBatch.Draw(sceneTarget, boxToDraw, white);
    }

    public void Draw(Texture2D pixelTexture, Rectangle rectangle, Color color)
    {
        _spriteBatch.Draw(pixelTexture, rectangle, color);
    }

    public void Draw(Texture2D charTexture, Vector2 vector, Color color)
    {
        _spriteBatch.Draw(charTexture, vector, null, color, 0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0f);
    }

    public void DrawMouse()
    {
        _spriteBatch.Draw(GFW.MouseTextures[MouseInput.Current_Cursor], MouseInput.MouseVirtualPosition(4), Color.White);
    }
}
