using Starlight.AnimeMatrix;

var mat = new AnimeMatrix();

// mat.SetDisplayState(true);
// mat.SetBrightness(BrightnessMode.Off);
//
// for (var j = 0; j < 256; j++)
// {
//     for (var i = 0; i < mat.LedCount; i++)
//     {
//         mat.SetLedLinear(i, (byte)j);
//     }
//
//     Console.WriteLine(j);
//     Console.ReadLine();
//     mat.Present();
// }

// mat.SendRaw(0xC0, 0x01, 0x80);

// for (var j = 0; j <= 255; j++)
// for (var i = 0; i < mat.Columns(0); i++)
// {
//     mat.SetLedLinear(i, (byte)j);
//     mat.Present();
// }
static void PQtoXY(int p, int q)
{
    
}

mat.Clear();
mat.SetBrightness(BrightnessMode.Dim);

for (var y = 0; y < mat.Rows; y++)
{
    var cols = mat.Columns(y);

    for (var x = 0; x < cols; x++)
    {
        if (x == 0 || x == cols - 1 || y == 0 || y == mat.Rows - 1)
        {
            mat.SetLedPlanar(x, y, 0xFF);
        }
    }
}

mat.Present();


// while (true)
// {
//     for (var row = 0; row < 61; row++)
//     {
//         mat.Clear();
//         var start = mat.RowToLinearAddress(row);
//         for (var i = start; i <= start + mat.Columns(row); i++)
//         {
//             mat.SetLedLinear(i, 0xFF);
//             mat.Present();
//         }
//         
//         for (var i = start; i <= start + mat.Columns(row); i++)
//         {
//             mat.SetLedLinear(i, 0x00);
//             mat.Present();
//         }
//     }
//     mat.Clear(true);
//     Thread.Sleep(500);
//     
//     for (var row = 60; row >= 0; row--)
//     {
//         mat.Clear();
//         var start = mat.RowToLinearAddress(row);
//         for (var i = start; i <= start + mat.Columns(row); i++)
//         {
//             mat.SetLedLinear(i, 0xFF);
//         }
//         mat.Present();
//     } 
//     
//     mat.Clear(true);
//     Thread.Sleep(500);
// }