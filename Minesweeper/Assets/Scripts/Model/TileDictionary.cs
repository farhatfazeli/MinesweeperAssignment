using System.Collections.Generic;
using System.Linq;

namespace Minesweeper.Model
{
    public class TileDictionary
    {
        public List<GC> ListOfCoordinates => _tileDictionary.Keys.ToList();
        public List<TileModel> ListOfTiles => _tileDictionary.Values.ToList();

        private readonly Dictionary<GC, TileModel> _tileDictionary;
        public TileDictionary()
        {
            _tileDictionary = new Dictionary<GC, TileModel>();
        }

        public void Add(GC gC, TileModel tileModel)
        {
            if (!_tileDictionary.ContainsKey(gC))
                _tileDictionary.Add(gC, tileModel);
        }

        public TileModel GetTile(GC gC)
        {
            return _tileDictionary.GetValueOrDefault(gC);
        }
    }
}

