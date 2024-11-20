import './App.css';
import React, { useState, useEffect } from "react";
import FileContainer from './FileContainer/FileContainer';
import FileUploader from './FileUploader/FileUploader';
import { getAllFiles, deleteFile, uploadFile } from './Services/FilesService';
import TablePanel from './TablePanel/TablePanel';

function App() {
  const [files, setFiles] = useState([]);
  const [loading, setLoading] = useState(true);

  const [isTableOpened, setIsTableOpendet] = useState(false);
  const [selectedFileId, setSelectedFileId] = useState(null);
  const [selectedFileName, setSelectedFileName] = useState(null);

  const fetchFiles = async () => {
    const files = await getAllFiles();
    console.log(files);
    setFiles(files);
    setLoading(false);
  };

  useEffect(() => {
    fetchFiles();
  }, []);

  const handleDelete = async (id) => {
    await deleteFile(id);
    fetchFiles();
  };

  const handleUpload = async (formData) => {
    const response = await uploadFile(formData);
    fetchFiles();
    return response;
  };

  const handleViewTable = (fileId, fileName) => {
    setSelectedFileId(fileId);
    setSelectedFileName(fileName);
    setIsTableOpendet(true);
  };

  const handleCloseTable = () => {
    setSelectedFileId(null);
    setSelectedFileName(null);
    setIsTableOpendet(false);
  };

  return (
    <>
      {loading ? (
        <p>Loading...</p>
      ) : (
        < FileContainer files={files}
          handleDelete={handleDelete}
          onViewTable={handleViewTable}
        />
      )}
      < FileUploader onUpload={handleUpload} />
      {isTableOpened && <TablePanel fileId={selectedFileId} fileName={selectedFileName} onClose={handleCloseTable} />}
    </>
  )
}

export default App;