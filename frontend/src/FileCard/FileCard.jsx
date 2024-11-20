import React from "react";
import "./FileCard.css";
import xlsxIcon from "../assets/xlsx_icon.png";
import { downloadFile } from "../Services/FilesService";

const FileCard = ({ sheetFile, onDelete, onViewTable }) => {
    const {
        fileId,
        fileName,
        publicationDate,
        bank: { bankName },
        period: { startDate, endDate },
    } = sheetFile;

    const handleDelete = async () => {
        await onDelete(fileId);
    }

    const handleViewTable = () => {
        onViewTable(fileId, fileName);
    };

    const handleDownload = async () => {
        await downloadFile(fileId, fileName);
    };

    return (
        <div className="file-card">

            <div className="file-card-image">
                <img
                    src={xlsxIcon}
                    alt="File Thumbnail"
                    className="file-card-thumbnail"
                />
            </div>

            <div className="file-card-content">
                <h3 className="file-card-title">{fileName}</h3>
                <div className="file-card-bank-period">
                    <span className="file-card-bank">{bankName}</span>
                    <span className="file-card-period">
                        {startDate} - {endDate}
                    </span>
                </div>
            </div>

            <div className="file-card-date">{publicationDate}</div>

            <div className="file-card-actions">
                <button className="file-card-delete" onClick={handleDelete}>
                    Delete
                </button>
                <button className="file-card-view" onClick={handleViewTable}>
                    View Table
                </button>
                <button className="file-card-download" onClick={handleDownload}>
                    Download
                </button>
            </div>

        </div>
    );
};

export default FileCard;
