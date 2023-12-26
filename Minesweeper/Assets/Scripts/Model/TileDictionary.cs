using System.Collections.Generic;
using System.Linq;

namespace Minesweeper.Model
{
    public class TileDictionary
    {
        public List<GC> ListOfCoordinates => _tileDictionary.Keys.ToList();
        public List<TileModel> ListOfTiles => _tileDictionary.Values.ToList();

        private readonly Dictionary<GC, TileModel> _tileDictionary;
        private readonly BoardModel _boardModel;

        public TileDictionary(BoardModel boardModel)
        {
            _tileDictionary = new Dictionary<GC, TileModel>();
            _boardModel = boardModel;
        }

        public void Add(GC gC, TileModel tileModel)
        {
            if (!_tileDictionary.ContainsKey(gC))
                _tileDictionary.Add(gC, tileModel);
        }

        public TileModel GetTile(GC gC)
        {
            GC wrappedGC = gC;
            wrappedGC.X = (gC.X + _boardModel.Width) % _boardModel.Width;
            wrappedGC.Z = (gC.Z + _boardModel.Height) % _boardModel.Height;
            return _tileDictionary.GetValueOrDefault(wrappedGC);
        }
    }
}

