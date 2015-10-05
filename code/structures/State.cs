﻿using System;
using System.Runtime.InteropServices;


namespace ManagedX.Input.XInput
{

	/// <summary>Contains the state of an XInput controller.</summary>
	[StructLayout( LayoutKind.Sequential, Pack = 4, Size = 16 )]
	public struct State : IEquatable<State>
	{
		
		private int packetNumber;
		private GamePad state;


		/// <summary>Gets the state packet number. The packet number indicates whether there have been any changes in the state of the controller.
		/// If the packet number [...] is the same in sequentially returned <see cref="State"/> structures, the controller state has not changed.</summary>
		public int PacketNumber
		{
			get { return packetNumber; }
		}


		/// <summary>Gets an <see cref="GamePad"/> structure containing the state of an XInput Controller.</summary>
		public GamePad GamePadState
		{
			get { return state; }
		}


		/// <summary>Returns the hash code of the <see cref="GamePadState"/>.</summary>
		/// <returns>Returns the hash code of the <see cref="GamePadState"/>.</returns>
		public override int GetHashCode()
		{
			return state.GetHashCode();
		}


		/// <summary>Returns a value indicating whether this <see cref="State"/> structure equals another structure of the same type.</summary>
		/// <param name="other">A <see cref="State"/> structure.</param>
		/// <returns>Returns true if the <paramref name="other"/> structure equals this <see cref="State"/> structure, otherwise returns false.</returns>
		public bool Equals( State other )
		{
			return ( packetNumber == other.packetNumber ) && state.Equals( other.state );
		}


		/// <summary>Returns a value indicating whether this <see cref="State"/> structure is equivalent to an object.</summary>
		/// <param name="obj">An object; if null, it is replaced with the <see cref="Empty"/> structure.</param>
		/// <returns>Returns true if the specified object is a <see cref="State"/> structure equal to this structure, otherwise returns false.</returns>
		public override bool Equals( object obj )
		{
			if( obj == null )
				return this.Equals( Empty );

			return ( obj is State ) && this.Equals( (State)obj );
		}


		/// <summary>The empty <see cref="State"/>.</summary>
		public static readonly State Empty = new State();


		#region Operators


		/// <summary></summary>
		public static bool operator ==( State state, State other )
		{
			return state.Equals( other );
		}

		
		/// <summary></summary>
		public static bool operator !=( State state, State other )
		{
			return !state.Equals( other );
		}


		#endregion


	}

}
