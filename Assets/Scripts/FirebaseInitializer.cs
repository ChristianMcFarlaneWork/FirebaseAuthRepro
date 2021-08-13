using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.UI;

public class FirebaseInitializer : MonoBehaviour
{

	#region Variables

	[Header("Auth UI")]
	[SerializeField] private Image _authImage;
	[SerializeField] private Text _authText;

	[Header("Linkages")]
	[SerializeField] private FirestoreTest _firestoreTester;
	[SerializeField] private StorageTest _storageTester;

	// Firebase References
	public FirebaseAuth auth { get; private set; }

	#endregion

	#region Start

	public void Start()
	{
		// Set our UI to default
		TestUtils.SetUI(_authImage, _authText, RequestState.NotSent, "Awaiting Initialization");

		// Initialize firebase
		InitializeFirebase();
	}

	#endregion

	#region Initialize Firebase

	private void InitializeFirebase()
	{
		// Initialize Firebase
		FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {

			// If we are available, init our modules
			if (task.Result == DependencyStatus.Available)
				InitializeAuth();
			else
				Debug.LogError($"Could not resolve all Firebase dependencies: {task.Result}");
		});
	}

	private void InitializeAuth()
	{
		// Just log our User in as a guest
		auth = FirebaseAuth.DefaultInstance;

		// Then Attempt to log our User in as a guest
		TestUtils.SetUI(_authImage, _authText, RequestState.Sending, "Logging In");
		auth.SignInAnonymouslyAsync().ContinueWithOnMainThread(task => {

			if (task.IsCanceled || task.IsFaulted)
				TestUtils.SetUI(_authImage, _authText, RequestState.Fail, task.Exception);
			else {
				TestUtils.SetUI(_authImage, _authText, RequestState.Succeed, "User Successfully logged in");
				_firestoreTester.RunFirestoreTests();
				_storageTester.RunStorageTests();
			}
		});
	}

	#endregion

}
