import './FileUploader.css';

function FileUploader({ onUpload }) {

    const handleUpload = async (e) => {
        e.preventDefault();
        const response = await onUpload(new FormData(e.target));

        const messageEl = document.querySelector(".result-message");
        messageEl.innerHTML = response.resultMessage;
    };

    return (
        <div className='file-uploader'>
            <form action='#' method='post' onSubmit={handleUpload}>
                <label htmlFor='select-files'>Select file(s)</label>
                <br /> <br />
                <input type='file' id='select-files' required name='fileToUpload' />
                <br /> <br />
                <input type='submit' value='Upload' className='button' />
            </form>
            <p className='result-message'></p>
        </div>
    )
}

export default FileUploader;