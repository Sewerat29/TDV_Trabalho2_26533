using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho2_Tecnicas_26533
{
    internal class GameManager
    {
        private PLayer _player;
        public void Init()
        {
            _player = new();
        }
        
        public void Update()
        {
            InputManager.Update();  
            _player.Update();
        }

        public void Draw()
        {
            _player.Draw();
        }

    }
}
