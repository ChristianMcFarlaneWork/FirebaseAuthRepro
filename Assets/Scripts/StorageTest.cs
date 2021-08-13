using Firebase;
using Firebase.Extensions;
using Firebase.Storage;
using UnityEngine;
using UnityEngine.UI;

public class StorageTest : MonoBehaviour
{

	#region Variables

	[Header("Storage UI")]
	[SerializeField] private Image _readStorageImage;
	[SerializeField] private Text _readStorageText;
	[SerializeField] private Image _writeStorageImage;
	[SerializeField] private Text _writeStorageText;

	[Header("Storage Settings")]
	[SerializeField] private LogLevel _storageLogLvl = LogLevel.Error;
	[Space]
	[SerializeField] private string _bucketPath = "gs://YOUR-FIREBASE-BUCKET/";
	[SerializeField] private string _storageReadFilePath = "ImageToRead.bytes";
	[SerializeField] private string _storageWriteFilePath = "ImageToWrite.bytes";
	[SerializeField] private TextAsset _assetToSendToServer = null;


	public FirebaseStorage storage { get; private set; }
	public StorageReference bucket { get; private set; }

	#endregion

	#region OnEnable / OnDisable

	private void OnEnable()
	{
		// Set our UI
		TestUtils.SetUI(_readStorageImage, _readStorageText, RequestState.NotSent, "Awaiting Request");
		TestUtils.SetUI(_writeStorageImage, _writeStorageText, RequestState.NotSent, "Awaiting Request");
	}

	#endregion

	#region Initialize

	public void RunStorageTests()
	{
		// Attempt to initialize our storage settings
		storage = FirebaseStorage.DefaultInstance;
		bucket = storage.GetReferenceFromUrl(_bucketPath);
		storage.LogLevel = _storageLogLvl;

		// Attempt Reading and Writing from Firebase
		AttemptReadWriteStorage();
	}

	#endregion

	#region Read / Write Storage

	private void AttemptReadWriteStorage()
	{
		AttemptReadStorage();
		AttemptWriteStorage();
	}

	private void AttemptReadStorage()
	{
		// Then do a simple request to access a document
		TestUtils.SetUI(_readStorageImage, _readStorageText, RequestState.Sending, $"Requesting to load content '{_storageReadFilePath}'");

		// Request our document, and just log if we succeed or fail
		bucket.Child(_storageReadFilePath).GetBytesAsync(8192 * 1024).ContinueWithOnMainThread(readTask => {

			// Then if we succeed or fail, display in the UI
			if (readTask.IsCanceled || readTask.IsFaulted)
				TestUtils.SetUI(_readStorageImage, _readStorageText, RequestState.Fail, readTask.Exception);
			else
				TestUtils.SetUI(_readStorageImage, _readStorageText, RequestState.Succeed, "Byte Download Succeeded");

			readTask.Dispose();
		});
	}

	private void AttemptWriteStorage()
	{
		// Then do a simple request to write the timestamp to a document
		TestUtils.SetUI(_writeStorageImage, _writeStorageText, RequestState.Sending, $"Requesting to Write document '{_storageWriteFilePath}'");

		// Set our timestamp onto our server
		bucket.Child(_storageWriteFilePath).PutBytesAsync(_assetToSendToServer.bytes).ContinueWithOnMainThread(writeTask => {

			// Then if we succeed or fail, display in the UI
			if (writeTask.IsCanceled || writeTask.IsFaulted)
				TestUtils.SetUI(_writeStorageImage, _writeStorageText, RequestState.Fail, writeTask.Exception);
			else
				TestUtils.SetUI(_writeStorageImage, _writeStorageText, RequestState.Succeed, "Bytes Uploaded Succeeded");

			writeTask.Dispose();
		});
	}

	#endregion

}
