using SevenWestMedia.ApiClient.Library.Models;

namespace SevenWestMedia.ApiClient.Console.ViewModels
{
    public class ViewModelMapper : IMapper<Person, PeopleViewModel>
    {
        public PeopleViewModel Map(PeopleViewModel viewModel, Person person)
        {
            MapNamesForId42(person, viewModel);
            MapFirstNamesAged23(person, viewModel);
            MapGendersByAge(person, viewModel);

            return viewModel;
        }
        private static void MapNamesForId42(Person person, PeopleViewModel viewModel)
        {
            if (person.Id != 42) return;
            viewModel.Id = person.Id;
            viewModel.FirstName = person.FirstName;
            viewModel.LastName = person.LastName;
        }
        private static void MapFirstNamesAged23(Person person, PeopleViewModel viewModel)
        {
            if (person.Age != 23) return;
            viewModel.Age = person.Age;
            viewModel.FirstNames.Add(person.FirstName);
        }

        private static void MapGendersByAge(Person person, PeopleViewModel viewModel)
        {
            if (!viewModel.GendersByAge.TryGetValue(person.Age, out var gendersViewModel))
            {
                gendersViewModel = new GendersViewModel();
                viewModel.GendersByAge.Add(person.Age, gendersViewModel);
            }

            AddPersonToGendersViewModel(person, gendersViewModel);
        }

        private static void AddPersonToGendersViewModel(Person person, GendersViewModel gendersViewModel)
        {
            var gender = person.Gender.ToUpper();

            gendersViewModel.Age = person.Age;

            if (gendersViewModel.Genders.TryAdd(gender, 1))
                return;

            gendersViewModel.Genders[gender] += 1;
        }
    }
}