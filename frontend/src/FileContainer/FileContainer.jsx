import FileCard from "../FileCard/FileCard"

const FileContainer = ({ files, handleDelete, onViewTable }) => {
    return (
        <div className="file-container" style={{ display: "flex", flexDirection: "column", gap: "16px" }}>
            {files.map((file) => (
                <FileCard
                    key={file.FileId}
                    sheetFile={file}
                    onDelete={handleDelete}
                    onViewTable={onViewTable}
                />
            ))}
        </div>
    );
};

export default FileContainer;
