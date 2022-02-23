﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Microsoft.Xna.Framework.Input;

namespace Blish_HUD.Input {

    /// <summary>
    /// Allows for actions to be ran as the result of a provided key combination.
    /// </summary>
    public class KeyBinding {

        /// <summary>
        /// Fires when the <see cref="KeyBinding"/> is triggered.
        /// </summary>
        public event EventHandler<EventArgs> Activated;

        protected void OnActivated(EventArgs e) {
            Activated?.Invoke(this, e);
        }

        /// <summary>
        /// The primary key in the binding.
        /// </summary>
        public Keys PrimaryKey { get; set; }

        /// <summary>
        /// Any combination of <see cref="ModifierKeys"/> required to be pressed
        /// in addition to the <see cref="PrimaryKey"/> for the <see cref="KeyBinding"/> to fire.
        /// </summary>
        public ModifierKeys ModifierKeys { get; set; }

        private bool _enabled;

        /// <summary>
        /// If <c>true</c>, the <see cref="KeyBinding"/> will be enabled and can be triggered by
        /// the specified key combinations.
        /// </summary>
        public bool Enabled {
            get => _enabled;
            set {
                if (_enabled != value) {
                    if (value) {
                        KeyboardOnKeyStateChanged(null, null);
                        GameService.Input.Keyboard.KeyStateChanged += KeyboardOnKeyStateChanged;
                    } else {
                        GameService.Input.Keyboard.KeyStateChanged -= KeyboardOnKeyStateChanged;
                        GameService.Input.Keyboard.UnstageKeyBinding(this);
                    }

                    _enabled = value;

                    Reset();
                }
            }
        }

        /// <summary>
        /// If <c>true</c>, the <see cref="PrimaryKey"/> is not sent to the game when it is
        /// the final key pressed in the keybinding sequence.
        /// </summary>
        [JsonIgnore]
        public bool BlockSequenceFromGw2 { get; set; } = false;
        
        /// <summary>
        /// Indicates if the <see cref="KeyBinding"/> is actively triggered.
        /// If triggered with <see cref="ManuallyTrigger"/>(), this
        /// will report <c>true</c> for only a single frame.
        /// </summary>
        [JsonIgnore]
        public bool IsTriggering { get; private set; }

        public KeyBinding() { /* NOOP */ }

        public KeyBinding(Keys primaryKey) : this(ModifierKeys.None, primaryKey) { /* NOOP */ }

        public KeyBinding(ModifierKeys modifierKeys, Keys primaryKey) {
            this.ModifierKeys = modifierKeys;
            this.PrimaryKey   = primaryKey;
        }

        private void KeyboardOnKeyStateChanged(object sender, KeyboardEventArgs e) {
            if (this.PrimaryKey == Keys.None) return;

            CheckTrigger(GameService.Input.Keyboard.ActiveModifiers, GameService.Input.Keyboard.KeysDown);
        }

        private void Reset() {
            StopFiring();
        }

        private void Fire() {
            if (this.IsTriggering) return;

            this.IsTriggering = true;

            ManuallyTrigger();
        }

        private void StopFiring() {
            this.IsTriggering = false;
        }

        private void CheckTrigger(ModifierKeys activeModifiers, IEnumerable<Keys> pressedKeys) {
            if (GameService.Gw2Mumble.UI.IsTextInputFocused) return;

            if ((this.ModifierKeys & activeModifiers) == this.ModifierKeys) {
                if (this.BlockSequenceFromGw2) {
                    GameService.Input.Keyboard.StageKeyBinding(this);
                }

                if (pressedKeys.Contains(this.PrimaryKey)) {
                    Fire();
                    return;
                }
            } else {
                GameService.Input.Keyboard.UnstageKeyBinding(this);
            }

            StopFiring();
        }

        /// <summary>
        /// Gets a display string representing the <see cref="KeyBinding"/> suitable
        /// for display in the UI.
        /// </summary>
        public string GetBindingDisplayText() => KeysUtil.GetFriendlyName(this.ModifierKeys, this.PrimaryKey);

        /// <summary>
        /// Manually triggers the actions bound to this <see cref="KeyBinding"/>.
        /// </summary>
        public void ManuallyTrigger() {
            OnActivated(EventArgs.Empty);
        }

    }

}
