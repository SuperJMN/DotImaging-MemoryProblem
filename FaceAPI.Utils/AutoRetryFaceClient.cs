namespace FaceAPI.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Microsoft.ProjectOxford.Face;
    using Microsoft.ProjectOxford.Face.Contract;

    public class AutoRetryFaceClient : IFaceServiceClient
    {
        private readonly IFaceServiceClient innerClient;

        public AutoRetryFaceClient(IFaceServiceClient innerClient)
        {
            this.innerClient = innerClient;
        }

        public Task<AddPersistedFaceResult> AddFaceToFaceListAsync(string faceListId, string imageUrl, string userData = null, FaceRectangle targetFace = null)
        {
            return innerClient.AddFaceToFaceListAsync(faceListId, imageUrl, userData, targetFace);
        }

        public Task<AddPersistedFaceResult> AddFaceToFaceListAsync(string faceListId, Stream imageStream, string userData = null, FaceRectangle targetFace = null)
        {
            return innerClient.AddFaceToFaceListAsync(faceListId, imageStream, userData, targetFace);
        }

        public Task<AddPersistedFaceResult> AddPersonFaceAsync(string personGroupId, Guid personId, string imageUrl, string userData = null, FaceRectangle targetFace = null)
        {
            return innerClient.AddPersonFaceAsync(personGroupId, personId, imageUrl, userData, targetFace);
        }

        public Task<AddPersistedFaceResult> AddPersonFaceAsync(string personGroupId, Guid personId, Stream imageStream, string userData = null, FaceRectangle targetFace = null)
        {
            return innerClient.AddPersonFaceAsync(personGroupId, personId, imageStream, userData, targetFace);
        }

        public Task CreateFaceListAsync(string faceListId, string name, string userData)
        {
            return innerClient.CreateFaceListAsync(faceListId, name, userData);
        }

        public Task<CreatePersonResult> CreatePersonAsync(string personGroupId, string name, string userData = null)
        {
            return innerClient.CreatePersonAsync(personGroupId, name, userData);
        }

        public Task CreatePersonGroupAsync(string personGroupId, string name, string userData = null)
        {
            return innerClient.CreatePersonGroupAsync(personGroupId, name, userData);
        }

        public Task DeleteFaceFromFaceListAsync(string faceListId, Guid persistedFaceId)
        {
            return innerClient.DeleteFaceFromFaceListAsync(faceListId, persistedFaceId);
        }

        public Task DeleteFaceListAsync(string faceListId)
        {
            return innerClient.DeleteFaceListAsync(faceListId);
        }

        public Task DeletePersonAsync(string personGroupId, Guid personId)
        {
            return innerClient.DeletePersonAsync(personGroupId, personId);
        }

        public Task DeletePersonFaceAsync(string personGroupId, Guid personId, Guid persistedFaceId)
        {
            return innerClient.DeletePersonFaceAsync(personGroupId, personId, persistedFaceId);
        }

        public Task DeletePersonGroupAsync(string personGroupId)
        {
            return innerClient.DeletePersonGroupAsync(personGroupId);
        }

        public Task<Face[]> DetectAsync(string imageUrl, bool returnFaceId, bool returnFaceLandmarks, IEnumerable<FaceAttributeType> returnFaceAttributes = null)
        {
            return TaskExtensions.RetryTask(() => innerClient.DetectAsync(imageUrl, returnFaceId, returnFaceLandmarks, returnFaceAttributes), IsRateLimitExceeded);
        }

        public Task<Face[]> DetectAsync(Stream imageStream, bool returnFaceId, bool returnFaceLandmarks, IEnumerable<FaceAttributeType> returnFaceAttributes = null)
        {
            return TaskExtensions.RetryTask(() => innerClient.DetectAsync(imageStream, returnFaceId, returnFaceLandmarks, returnFaceAttributes), IsRateLimitExceeded);
        }

        public Task<SimilarFace[]> FindSimilarAsync(Guid faceId, Guid[] faceIds, int maxNumOfCandidatesReturned = 20)
        {
            return innerClient.FindSimilarAsync(faceId, faceIds, maxNumOfCandidatesReturned);
        }

        public Task<SimilarPersistedFace[]> FindSimilarAsync(Guid faceId, string faceListId, int maxNumOfCandidatesReturned = 20)
        {
            return innerClient.FindSimilarAsync(faceId, faceListId, maxNumOfCandidatesReturned);
        }

        public Task<FaceList> GetFaceListAsync(string faceListId)
        {
            return innerClient.GetFaceListAsync(faceListId);
        }

        public Task<Person> GetPersonAsync(string personGroupId, Guid personId)
        {
            return innerClient.GetPersonAsync(personGroupId, personId);
        }

        public Task<PersonFace> GetPersonFaceAsync(string personGroupId, Guid personId, Guid persistedFaceId)
        {
            return innerClient.GetPersonFaceAsync(personGroupId, personId, persistedFaceId);
        }

        public Task<PersonGroup> GetPersonGroupAsync(string personGroupId)
        {
            return innerClient.GetPersonGroupAsync(personGroupId);
        }

        public Task<PersonGroup[]> GetPersonGroupsAsync()
        {
            return innerClient.GetPersonGroupsAsync();
        }

        public Task<TrainingStatus> GetPersonGroupTrainingStatusAsync(string personGroupId)
        {
            return innerClient.GetPersonGroupTrainingStatusAsync(personGroupId);
        }

        public Task<Person[]> GetPersonsAsync(string personGroupId)
        {
            return innerClient.GetPersonsAsync(personGroupId);
        }

        public Task<GroupResult> GroupAsync(Guid[] faceIds)
        {
            return TaskExtensions.RetryTask(() => innerClient.GroupAsync(faceIds), IsRateLimitExceeded);
        }

        public Task<IdentifyResult[]> IdentifyAsync(string personGroupId, Guid[] faceIds, int maxNumOfCandidatesReturned)
        {
            return TaskExtensions.RetryTask(() => innerClient.IdentifyAsync(personGroupId, faceIds, maxNumOfCandidatesReturned), IsRateLimitExceeded);
        }

        public Task<FaceListMetadata[]> ListFaceListsAsync()
        {
            return innerClient.ListFaceListsAsync();
        }

        public Task TrainPersonGroupAsync(string personGroupId)
        {
            return innerClient.TrainPersonGroupAsync(personGroupId);
        }

        public Task UpdateFaceListAsync(string faceListId, string name, string userData)
        {
            return innerClient.UpdateFaceListAsync(faceListId, name, userData);
        }

        public Task UpdatePersonAsync(string personGroupId, Guid personId, string name, string userData = null)
        {
            return innerClient.UpdatePersonAsync(personGroupId, personId, name, userData);
        }

        public Task UpdatePersonFaceAsync(string personGroupId, Guid personId, Guid persistedFaceId, string userData = null)
        {
            return innerClient.UpdatePersonFaceAsync(personGroupId, personId, persistedFaceId, userData);
        }

        public Task UpdatePersonGroupAsync(string personGroupId, string name, string userData = null)
        {
            return innerClient.UpdatePersonGroupAsync(personGroupId, name, userData);
        }

        public Task<VerifyResult> VerifyAsync(Guid faceId1, Guid faceId2)
        {
            return innerClient.VerifyAsync(faceId1, faceId2);
        }

        public HttpRequestHeaders DefaultRequestHeaders => innerClient.DefaultRequestHeaders;

        private bool IsRateLimitExceeded(Exception arg)
        {
            var faceApiException = arg as FaceAPIException;
            if (faceApiException != null && faceApiException.ErrorCode == "RateLimitExceeded")
            {
                return true;
            }

            return false;
        }
    }

    public static class TaskExtensions
    {
        public static async Task<T> RetryTask<T>(Func<Task<T>> myTask, Func<Exception, bool> shouldRetry, int? retryCount = null)
        {
            int n = 1;
            var retry = true;

            while (true)
            {
                try
                {
                    return await myTask();
                }
                catch (Exception ex)
                {
                    if (shouldRetry?.Invoke(ex) ?? true)
                    {
                        Debug.WriteLine("Retrying...");

                        if (retryCount.HasValue)
                        {
                            if (retryCount == 0)
                            {
                                throw;
                            }

                            retryCount--;
                        }

                        await Task.Delay(TimeSpan.FromSeconds(n));
                        n = n * 2;
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        public static async Task RetryTask(Func<Task> myTask, Func<Exception, bool> shouldRetry, int? retryCount = null)
        {
            int n = 1;
            var hasExecuted = false;
            while (!hasExecuted)
            {
                try
                {
                    await myTask();
                    hasExecuted = true;
                }
                catch (Exception ex)
                {
                    if (shouldRetry?.Invoke(ex) ?? true)
                    {
                        Debug.WriteLine("Retrying...");

                        if (retryCount.HasValue)
                        {
                            if (retryCount == 0)
                            {
                                throw;
                            }

                            retryCount--;
                        }

                        await Task.Delay(TimeSpan.FromSeconds(n));
                        n = n * 2;
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }
    }
}