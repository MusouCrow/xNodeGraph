using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

#if UNITY_EDITOR
    using UnityEditor;
    using Sirenix.OdinInspector.Editor;
#endif

namespace Game.Graph {
    [Serializable]
    public class Blackboard {
        [Serializable]
        public class Unit {
            public string type;
            public string name;
            public Number number;
            public Bool boolean;
            public string text;
            public Vec3 vec3;
            public Col col;
            public Object obj;

            public object GetValue() {
                if (this.type == typeof(Number).ToString()) {
                    return this.number;
                }
                else if (this.type == typeof(Bool).ToString()) {
                    return this.boolean;
                }
                else if (this.type == typeof(string).ToString()) {
                    return this.text;
                }
                else if (this.type == typeof(Col).ToString()) {
                    return this.col;
                }
                else if (this.type == typeof(Vec3).ToString()) {
                    return this.vec3;
                }
                
                return this.obj;
            }
        }

        public Unit[] values;
    }

    #if UNITY_EDITOR
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

            EditorGUI.BeginChangeCheck();

            if (self.values == null) {
                self.values = new Blackboard.Unit[0];
            }

            for (int i = 0; i < self.values.Length; i++) {
                var unit = self.values[i];

                EditorGUILayout.BeginHorizontal();
                unit.name = EditorGUILayout.TextField(unit.name);

                if (unit.type == typeof(Number).ToString()) {
                    unit.number.value = EditorGUILayout.FloatField(unit.number.value);
                }
                else if (unit.type == typeof(Bool).ToString()) {
                    unit.boolean.value = EditorGUILayout.Toggle(unit.boolean.value);
                }
                else if (unit.type == typeof(string).ToString()) {
                    unit.text = EditorGUILayout.TextField(unit.text);
                }
                else if (unit.type == typeof(Col).ToString()) {
                    unit.col.value = EditorGUILayout.ColorField(unit.col.value);
                }
                else if (unit.type == typeof(Vec3).ToString()) {
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

            if (EditorGUI.EndChangeCheck()) {
                EditorUtility.SetDirty(Selection.activeObject);
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
            unit.type = type.ToString();
            
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
    #endif
}