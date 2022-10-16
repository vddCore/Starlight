using System.Numerics;
using Starlight.AnimeMatrix;

var mat = new AnimeMatrix();
mat.Clear();

var t = 0f;

while (true)
{
    for (var y = 0; y < mat.Rows; y++)
    {
        var cols = mat.Columns(y);
        
        for (var x = 0; x < cols; x++)
        {
            // var v = MathF.Sin(MathF.Sqrt(x * 60f) + MathF.Log10(y * 25f) + t);
            var v = MathF.Sin(MathF.Sqrt((x) * 15f) + t);
            // v -= MathF.Cos(MathF.Cbrt((mat.Rows - y) * 75f) + t);
            mat.SetLedPlanar(x, y, (byte)(255 * Math.Abs(v * 0.5f * MathF.PI)));
        }
    }

    mat.Present();

    t += 0.08f;
    Thread.Sleep(16);
}

