using Firebase;
using Firebase.Firestore;
using Firebase.Storage;
using System;
using UnityEngine;
using UnityEngine.UI;

#region Enum

public enum RequestState
{
	NotSent,
	Sending,
	Succeed,
	Fail
}

#endregion

public static class TestUtils
{

	#region Set UI

	public static void SetUI(Image image, Text text, RequestState request, Exception exception)
	{
		SetUI(image, text, request, GetExceptionMessage(exception));
	}

	public static void SetUI(Image image, Text text, RequestState requestState, string infoText)
	{
		// Set the colour of our image based off of the request state
		image.color = GetStateColour(requestState);
		text.text = infoText;
	}

	#endregion

	#region Utility

	public static string GetExceptionMessage(Exception exception)
	{
		// Get our Exception and see if we can drill down to it's base type
		if (exception is AggregateException aggregate)
			exception = aggregate.InnerException;

		// Then display our error differently depending on the type of exception we are
		switch (exception) {
			case StorageException storageExc:
				return $"Error Code '{storageExc.ErrorCode}' - Error '{storageExc.Message}'";

			case FirestoreException firestoreExc:
				return $"Error Code '{firestoreExc.ErrorCode}' - Error '{firestoreExc.Message}'";

			case FirebaseException firebaseExc:
				return $"Error Code '{firebaseExc.ErrorCode}' - Error '{firebaseExc.Message}'";

			default:
				return exception.Message;
		}
	}

	public static Color GetStateColour(RequestState state)
	{
		switch (state) {
			default:
			case RequestState.NotSent:
				return Color.gray;

			case RequestState.Sending:
				return Color.blue;

			case RequestState.Succeed:
				return Color.green;

			case RequestState.Fail:
				return Color.red;
		}
	}

	#endregion

}
