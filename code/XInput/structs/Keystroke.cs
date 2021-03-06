﻿using System;
using System.Runtime.InteropServices;

// THINKABOUTME - remove unicodeChar from GetHashCode and Equals, to allow user-implemented (and localized) support.


namespace ManagedX.Input.XInput
{

	/// <summary>Specifies keystroke data returned by XInputGetKeystroke.
	/// <para>This structure is equivalent to the <code>XINPUT_KEYSTROKE</code> structure (defined in XInput.h).</para>
	/// </summary>
	/// <remarks>https://msdn.microsoft.com/en-us/library/windows/desktop/microsoft.directx_sdk.reference.xinput_keystroke%28v=vs.85%29.aspx</remarks>
	[Win32.Source( "XInput.h", "XINPUT_KEYSTROKE" )]
	[StructLayout( LayoutKind.Sequential, Pack = 1, Size = 8 )]
	public struct Keystroke : IEquatable<Keystroke>
	{

		private VirtualKeyCode virtualKey;
		/// <summary>This member is unused and the value is zero.</summary>
		internal char unicodeChar;
		private KeyStates flags;
		private byte userIndex;
		private byte hidCode;



		/// <summary>Gets the virtual-key code of the key, button, or stick movement.</summary>
		public VirtualKeyCode VirtualKey { get { return virtualKey; } }


		/// <summary>Gets a value indicating the keyboard state at the time of the input event.</summary>
		public KeyStates State { get { return flags; } }


		/// <summary>Gets a value indicating whether a state flag is present.</summary>
		/// <param name="state">A <see cref="KeyStates"/> value.</param>
		/// <returns>Returns true if the specified <paramref name="state"/> is present, otherwise returns false.</returns>
		public bool IsSet( KeyStates state )
		{
			return ( flags & state ) == state;
		}


		/// <summary>Gets the index of the signed-in gamer associated with the device.</summary>
		public GameControllerIndex UserIndex { get { return (GameControllerIndex)userIndex; } }


		/// <summary>Gets the HID code corresponding to the input. If there is no corresponding HID code, this value is zero.</summary>
		public byte HidCode { get { return hidCode; } }

		
		// TODO - add a Char property ?

		
		/// <summary>Returns a hash code for this <see cref="Keystroke"/> structure.</summary>
		/// <returns>Returns a hash code for this <see cref="Keystroke"/> structure.</returns>
		public override int GetHashCode()
		{
			return virtualKey.GetHashCode() ^ unicodeChar.GetHashCode() ^ flags.GetHashCode() ^ userIndex.GetHashCode() ^ hidCode.GetHashCode();
		}


		/// <summary>Returns a value indicating whether this <see cref="Keystroke"/> structure equals another structure of the same type.</summary>
		/// <param name="other">A <see cref="Keystroke"/> structure.</param>
		/// <returns>Returns true if this <see cref="Keystroke"/> structure equals the <paramref name="other"/> structure, otherwise returns false.</returns>
		public bool Equals( Keystroke other )
		{
			return ( virtualKey == other.virtualKey ) && ( unicodeChar == other.unicodeChar ) && ( flags == other.flags ) && ( userIndex == other.userIndex ) && ( hidCode == other.hidCode );
		}

		
		/// <summary>Returns a value indicating whether this <see cref="Keystroke"/> structure is equivalent to an object.</summary>
		/// <param name="obj">An object.</param>
		/// <returns>Returns true if the specified object is a <see cref="Keystroke"/> structure equivalent to this structure, otherwise returns false.</returns>
		public override bool Equals( object obj )
		{
			return ( obj is Keystroke ) && ( this.Equals( (Keystroke)obj ) );
		}
		

		/// <summary>The empty <see cref="Keystroke"/> structure.</summary>
		public static readonly Keystroke Empty;


		#region Operators

		/// <summary>Equality comparer.</summary>
		/// <param name="keystroke">A <see cref="Keystroke"/> structure.</param>
		/// <param name="other">A <see cref="Keystroke"/> structure.</param>
		/// <returns>Returns true if the structures are equal, otherwise returns false.</returns>
		public static bool operator ==( Keystroke keystroke, Keystroke other )
		{
			return keystroke.Equals( other );
		}


		/// <summary>Inequality comparer.</summary>
		/// <param name="keystroke">A <see cref="Keystroke"/> structure.</param>
		/// <param name="other">A <see cref="Keystroke"/> structure.</param>
		/// <returns>Returns true if the structures are not equal, otherwise returns false.</returns>
		public static bool operator !=( Keystroke keystroke, Keystroke other )
		{
			return !keystroke.Equals( other );
		}

		#endregion Operators

	}

}
