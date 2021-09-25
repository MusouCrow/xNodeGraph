using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

#if UNITY_EDITOR
    using UnityEditor;
#endif

namespace Game.Graph {
    [Serializable]
    public class Blackboard {
        [Serializable]
        public class Unit {
            public string name;
            public Number number;
            public Bool boolean;
            public string text;
            public Vec3 vec3;
            public Col col;
            public Object obj;

            public object GetValue() {
                if (this.number != null) {
                    return this.number;
                }
                else if (this.boolean != null) {
                    return this.boolean;
                }
                else if (this.text != null) {
                    return this.text;
                }
                else if (this.col != null) {
                    return this.col;
                }
                else if (this.vec3 != null) {
                    return this.vec3;
                }
                
                return this.obj;
            }
        }

        public Unit[] values;
    }

    public class BlackboardDrawer : OdinValueDrawer<Blackboard> {
        private static Type[] Types = new Type[] {
            typeof(Number),
            typeof(Bool),
            typeof(string),
            typeof(Vec3),
            typeof(Col),
            typeof(Object)
        };

        protected override void DrawPropertyLayout(GUIContent label) {
            int removeIndex = -1;
            Blackboard self = this.ValueEntry.SmartValue;
            
            GUILayout.Label("Blackboard", EditorStyles.boldLabel);

            for (int i = 0; i < self.values.Length; i++) {
                var unit = self.values[i];

                EditorGUILayout.BeginHorizontal();
                unit.name = EditorGUILayout.TextField(unit.name);

                if (unit.number != null) {
                    unit.number.value = EditorGUILayout.FloatField(unit.number.value);
                }
                else if (unit.boolean != null) {
                    unit.boolean.value = EditorGUILayout.Toggle(unit.boolean.value);
                }
                else if (unit.text != null) {
                    unit.text = EditorGUILayout.TextField(unit.text);
                }
                else if (unit.col != null) {
                    unit.col.value = EditorGUILayout.ColorField(unit.col.value);
                }
                else if (unit.vec3 != null) {
                    unit.vec3.value = EditorGUILayout.Vector3Field("", unit.vec3.value);
                }
                else {
                    unit.obj = EditorGUILayout.ObjectField(unit.obj, typeof(Object), true);
                }

                if (GUILayout.Button("X")) {
                    removeIndex = i;
                }

                EditorGUILayout.EndHorizontal();
            }

            if (GUILayout.Button("Add Variable")) {
                this.SelectTypes();
            }

            if (removeIndex > -1) {
                this.RemoveUnit(removeIndex);
            }
        }

        private void RemoveUnit(int index) {
            Blackboard self = this.ValueEntry.SmartValue;
            var values = new Blackboard.Unit[self.values.Length - 1];
            int n = 0;

            for (int i = 0; i < self.values.Length; i++) {
                if (i != index) {
                    values[n] = self.values[i];
                    n++;
                }
            }

            self.values = values;
        }

        private void AddUnit(Type type) {
            Blackboard self = this.ValueEntry.SmartValue;
            var unit = new Blackboard.Unit();
            unit.text = null;
            
            if (type == typeof(Number)) {
                unit.number = new Number();
            }
            else if (type == typeof(Bool)) {
                unit.boolean = new Bool();
            }
            else if (type == typeof(Vec3)) {
                unit.vec3 = new Vec3();
            }
            else if (type == typeof(Col)) {
                unit.col = new Col();
            }
            else if (type == typeof(string)) {
                unit.text = "";
            }
            
            var values = new Blackboard.Unit[self.values.Length + 1];
            self.values.CopyTo(values, 0);
            values[self.values.Length] = unit;
            self.values = values;
        }

        private void SelectTypes() {
            var typeSelector = new GenericSelector<Type>("选择变量类型", false, Types);
            typeSelector.EnableSingleClickToSelect();
            typeSelector.SelectionChanged += this.AddType;
            typeSelector.ShowInPopup();
        }

        private void AddType(IEnumerable<Type> types) {
            var type = types.FirstOrDefault();

            if (type != null) {
                this.AddUnit(type);
            }
        }
    }
}