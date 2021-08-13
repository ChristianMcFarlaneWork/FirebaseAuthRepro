using Firebase;
using Firebase.Extensions;
using Firebase.Firestore;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirestoreTest : MonoBehaviour
{

	#region Variables

	[Header("Firestore UI")]
	[SerializeField] private Image _readFirestoreImage;
	[SerializeField] private Text _readFirestoreText;
	[Space]
	[SerializeField] private Image _writeFirestoreImage;
	[SerializeField] private Text _writeFirestoreText;

	[Header("Firestore Settings")]
	[SerializeField] private LogLevel _firestoreLogLvl = LogLevel.Error;
	[Space]
	[SerializeField] private string _firestoreWriteDocumentPath = "Collection/WriteDocument";
	[SerializeField] private string _firestoreReadDocumentPath = "Collection/ReadDocument";

	public FirebaseFirestore firestore { get; private set; }

	#endregion

	#region OnEnable

	private void OnEnable()
	{
		// Default our Text
		TestUtils.SetUI(_readFirestoreImage, _readFirestoreText, RequestState.NotSent, "Awaiting Request");
		TestUtils.SetUI(_writeFirestoreImage, _writeFirestoreText, RequestState.NotSent, "Awaiting Request");
	}

	#endregion

	#region Initialize Firestore

	public void RunFirestoreTests()
	{
		FirebaseFirestore.LogLevel = _firestoreLogLvl;
		firestore = FirebaseFirestore.DefaultInstance;

		AttemptReadWriteFirestore();
	}

	#endregion

	#region Excecute Read / Write Tests

	private void AttemptReadWriteFirestore()
	{
		AttemptReadFirestore();
		AttemptWriteFirestore();
	}

	private void AttemptReadFirestore()
	{
		// Do a simple request to access a document
		TestUtils.SetUI(_readFirestoreImage, _readFirestoreText, RequestState.Sending, $"Requesting to read document '{_firestoreReadDocumentPath}'");

		// Request our document, and just log if we succeed or fail
		firestore.Document(_firestoreReadDocumentPath).GetSnapshotAsync().ContinueWithOnMainThread(readTask => {

			// Then if we succeed or fail, display in the UI
			if (readTask.IsCanceled || readTask.IsFaulted)
				TestUtils.SetUI(_readFirestoreImage, _readFirestoreText, RequestState.Fail, readTask.Exception);
			else
				TestUtils.SetUI(_readFirestoreImage, _readFirestoreText, RequestState.Succeed, "Read Succeeded");

			readTask.Dispose();
		});
	}

	private void AttemptWriteFirestore()
	{
		// Then do a simple request to write the timestamp to a document
		TestUtils.SetUI(_writeFirestoreImage, _writeFirestoreText, RequestState.Sending, $"Requesting to Write document '{_firestoreWriteDocumentPath}'");

		// Set our timestamp onto our server
		Dictionary<string, object> documentData = new Dictionary<string, object>() { { "Server Timestamp", FieldValue.ServerTimestamp } };
		firestore.Document(_firestoreWriteDocumentPath).SetAsync(documentData).ContinueWithOnMainThread(writeTask => {

			// Then if we succeed or fail, display in the UI
			if (writeTask.IsCanceled || writeTask.IsFaulted)
				TestUtils.SetUI(_writeFirestoreImage, _writeFirestoreText, RequestState.Fail, writeTask.Exception);
			else
				TestUtils.SetUI(_writeFirestoreImage, _writeFirestoreText, RequestState.Succeed, "Write Succeeded");

			writeTask.Dispose();
		});
	}

	#endregion

}
