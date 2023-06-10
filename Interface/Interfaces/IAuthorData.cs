using Interface.DTO;

namespace Interface.Interfaces;

public interface IAuthorData : IBase<AuthorDTO>, IValidateData
{
    public IEnumerable<AuthorDTO> GetByIds(IEnumerable<byte> authorIds);
}