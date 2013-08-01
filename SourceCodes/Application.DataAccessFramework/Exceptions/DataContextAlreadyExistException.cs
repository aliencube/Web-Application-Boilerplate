﻿using System;
using System.Runtime.Serialization;

namespace Application.DataAccessFramework.Exceptions
{
	/// <summary>
	/// The exception that is thrown when data context has not been changed.
	/// </summary>
	public class DataContextAlreadyExistException : ApplicationException
	{
		/// <summary>
		/// Initialises a new instance of the DataContextAlreadyExistException object.
		/// </summary>
		public DataContextAlreadyExistException()
			: base()
		{
		}

		/// <summary>
		/// Initialises a new instance of the DataContextAlreadyExistException object with serialized data.
		/// </summary>
		/// <param name="info">The object that holds the serialized object data.</param>
		/// <param name="context">The contextual information about the source or destination.</param>
		public DataContextAlreadyExistException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		/// <summary>
		/// Initialises a new instance of the DataContextAlreadyExistException object with a specified error message.
		/// </summary>
		/// <param name="message">A message that describes the error.</param>
		public DataContextAlreadyExistException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Initialises a new instance of the DataContextAlreadyExistException object with a specified error message and a reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="innerException">The exception that is the cause of the current exception. If the innerException parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
		public DataContextAlreadyExistException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
