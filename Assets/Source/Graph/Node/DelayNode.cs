using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Graph {
    public class DelayNode : FlowNode {
        public async override Task<object> RunAsync(Dictionary<BaseNode, object> cache) {
            await Task.Delay(3000);

            return null;
        }
    }
}